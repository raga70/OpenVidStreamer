using Common.MBcontracts;
using MassTransit;
using OpenVisStreamer.VideoLibrary.Model;
using OpenVisStreamer.VideoLibrary.Model.Entities;
using OpenVisStreamer.VideoLibrary.Model.Mappers;

namespace OpenVisStreamer.VideoLibrary.Services;

public class VideoService
{
    private readonly IRequestClient<RecommendationVideoRequest> _VideoRecommendationsRequestClient;
    private readonly VideoMapper _videoMapper = new();
    private readonly VideoRepository _videoRepository;
    
    public VideoService(VideoRepository videoRepository, IBus bus)
    {
        _VideoRecommendationsRequestClient = bus.CreateRequestClient<RecommendationVideoRequest>();
        _videoRepository = videoRepository;
    }
    
    
    public async Task<VideoDTO> GetVideoById(Guid videoId)
    {
        var video = await _videoRepository.GetVideoById(videoId);
        return _videoMapper.VideoToVideoDto(video);
    }


    public async Task<List<VideoDTO>> GetRecommendedVideos(Guid userId,VideoCategory category)
    {
        var videoRecommendationsResponse = await _VideoRecommendationsRequestClient.GetResponse<RecommendationVideoResponse>(new
        {
            UserId = userId,
            Category = category
        });
        var videos = await _videoRepository.GetVideosByVideoIds(videoRecommendationsResponse.Message.VideoIds);
        return videos.Select(_videoMapper.VideoToVideoDto).ToList();
    }
    
}