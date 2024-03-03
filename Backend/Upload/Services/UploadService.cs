﻿using Common.MBcontracts;
using MassTransit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Upload.Model.DTO;

namespace Upload.Services;

public class UploadService(IConfiguration configuration, IBus bus)
{
    private readonly char _pathSepator = Path.DirectorySeparatorChar;

    private readonly IRequestClient<UploadVideoRequest> _VideoUploadRequestClient = bus.CreateRequestClient<UploadVideoRequest>();
    
    public async Task UploadVideo(VideoUploadDTO videoMetadata, IFormFile videoFile, IFormFile thumbnailFile, string accId)
    {
        Guid newVideoId = Guid.NewGuid();
        var baseNFSpath = configuration.GetValue<string>("PVstorageBucketPath");
        var todayFolder = DateTime.Now.ToString("yyyyMMdd");

        if (!Directory.Exists(baseNFSpath + todayFolder))
        {
            Directory.CreateDirectory(baseNFSpath + todayFolder);
        }

        Directory.CreateDirectory(baseNFSpath + _pathSepator + todayFolder + _pathSepator + newVideoId);

        var videoDirectory = Path.Combine(baseNFSpath + _pathSepator + todayFolder + _pathSepator + newVideoId);

        var nonRenderedVideoPath = Path.Combine(videoDirectory, "notRendered.unknown");
        var thumbnailPath = Path.Combine(videoDirectory, thumbnailFile.FileName);


        using (var stream = new FileStream(nonRenderedVideoPath, FileMode.Create))
        {
            await videoFile.CopyToAsync(stream);
        }

        using (var stream = new FileStream(thumbnailPath, FileMode.Create))
        {
            await thumbnailFile.CopyToAsync(stream);
        }

        ConvertThumbnailToJpg(thumbnailPath, videoDirectory);
        
        
        //construct the videoDTO object
        UploadVideoRequest video = new UploadVideoRequest()
        {
            VideoId = newVideoId,
            Title = videoMetadata.Title,
            Description = videoMetadata.Description,
            Category = videoMetadata.Category,
            VideoUri = videoDirectory + _pathSepator + "playlist.m3u8",
            ThumbnailUri = videoDirectory + _pathSepator + "thumbnail.jpg",
            uploadedByAccoutId = Guid.Parse(accId),
            UploadDateTime = DateTime.Now,
            IsPublic = false
        };
        _VideoUploadRequestClient.Create(video);
        
        //todo: send to render service
        //todo: call account service to reward the user

    }

  
    
    

    /// <summary>
    /// convert the thumbnail to jpg from the unknown image format
    /// </summary>
    /// <param name="thumbnailPath"></param>
    /// <param name="videoDirectory"></param>
    public void ConvertThumbnailToJpg(string thumbnailPath, string videoDirectory)
    {
        using (var image = Image.Load(thumbnailPath))
        {
            image.Save(videoDirectory + _pathSepator + "thumbnail.jpg", new JpegEncoder { Quality = 90 });
        }
    }
    
    
    
    
    
}