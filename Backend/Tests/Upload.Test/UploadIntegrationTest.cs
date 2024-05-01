using Common.MBcontracts;
using Common.Model;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using OpenVisStreamer.VideoLibrary.Model.Mappers;
using OpenVisStreamer.VideoLibrary.Repository;
using OpenVisStreamer.VideoLibrary.Repository.EFC;
using OpenVisStreamer.VideoLibrary.Services;
using Render.Services;
using Upload.Model.DTO;
using Upload.Services;

public class MicroservicesIntegrationTests
{
    private readonly IConfiguration _configuration;
    private readonly Mock<IBus> _mockBus;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly UploadService _uploadService;
    private readonly RenderService _renderService;
    private readonly VideoService _videoService;
    private readonly VideoRepository _videoRepository;

    public MicroservicesIntegrationTests()
    {

        _mockBus = new Mock<IBus>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\..\..\..\Upload"))) //inject the real appsettings.json from upload ms
            .AddJsonFile("appsettings.Development.json").Build();

        _uploadService = new UploadService(_configuration, _mockBus.Object, _mockPublishEndpoint.Object);
        _renderService = new RenderService(_mockBus.Object, _mockPublishEndpoint.Object);

        var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var dbContext = new DatabaseContext(options);
        _videoRepository = new VideoRepository(dbContext);
    }

    
    /// <summary>
    /// simulates the process of uploading a video, not rendering it but simulating the flow of successful render, saving the metadata to video library, and then checking if the video waspublished successfully
    ///
    /// Masstransit is manually Mocked and the job of the message consumers is simulated
    /// </summary>
    [Fact]
    public async Task UploadAndRenderVideo_ReturnsSuccess()
    {
        // Arrange
        var videoMetadata = new VideoUploadDTO()
        {
            Title = "testTitle", Description = "testDescription", Category = VideoCategory.Other
        };
        var accId = new Guid().ToString();

        //Simulating video being uploaded
        // Download sample video and thumbnail (Cant use local files if running in CI)
        var httpClient = new HttpClient();
        var videoBytes =
            await httpClient.GetByteArrayAsync(
                "https://sample-videos.com/video321/mp4/720/big_buck_bunny_720p_1mb.mp4"); // 1MB real video of Big Buck Bunny
        var thumbnailBytes =
            await httpClient.GetByteArrayAsync(
                "https://cdn.pixabay.com/photo/2014/06/03/19/38/road-sign-361513_640.jpg"); // Sample thumbnail 

        var videoPath = Path.Combine(Path.GetTempPath(), "sample.mp4");
        var thumbnailPath = Path.Combine(Path.GetTempPath(), "sample.jpg");
        await File.WriteAllBytesAsync(videoPath, videoBytes);
        await File.WriteAllBytesAsync(thumbnailPath, thumbnailBytes);

        var videoFile = new FormFile(new FileStream(videoPath, FileMode.Open), 0, videoBytes.Length, "Data",
            "sample.mp4");
        var thumbnailFile = new FormFile(new FileStream(thumbnailPath, FileMode.Open), 0, thumbnailBytes.Length, "Data",
            "sample.jpg");





        //Capture the published messages for the VideoUploadRequest
        UploadVideoRequest VideoLibMSG = null;
        _mockPublishEndpoint.Setup(x => x.Publish<UploadVideoRequest>(It.IsAny<UploadVideoRequest>(), default))
            .Callback<UploadVideoRequest, CancellationToken>((message, token) => VideoLibMSG = message)
            .Returns(Task.CompletedTask);

        // Act
        await _uploadService.UploadVideo(videoMetadata, videoFile, thumbnailFile, accId);

        // Assert That the messages ware published
        _mockPublishEndpoint.Verify(x => x.Publish<UploadVideoRequest>(It.IsAny<UploadVideoRequest>(), default),
            Times.Once);
        _mockPublishEndpoint.Verify(x => x.Publish<RenderVideoRequest>(It.IsAny<RenderVideoRequest>(), default),
            Times.Once);







        //VideoLibrary  , simulate the work of MassTransit consumers

        //uploadVideoMetadata with public false;
        var video = UploadVideoMapper.UploadVideoRequestToVideo(VideoLibMSG);
        Assert.False(video.IsPublic);
        await _videoRepository.Create(video);

        //make video public (this will happen when the video is rendered, and a message is sent to the video library)
        var publicReturnedVideo = await _videoRepository.UpdateVideoToPublic(video.VideoId, 5);

        Assert.True(publicReturnedVideo is not null && publicReturnedVideo.VideoId == VideoLibMSG.VideoId &&
                    publicReturnedVideo.IsPublic);

    }

}