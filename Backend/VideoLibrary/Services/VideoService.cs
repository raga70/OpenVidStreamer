using Common.MBcontracts;
using Common.Model;
using MassTransit;
using OpenVisStreamer.VideoLibrary.Model;
using OpenVisStreamer.VideoLibrary.Model.Mappers;
using OpenVisStreamer.VideoLibrary.Repository;

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
    
    
    public async Task<VideoDTO?> GetVideoById(Guid videoId)
    {
        var video = await _videoRepository.GetVideoById(videoId);
        if (video is null)
            return null;
        
        return _videoMapper.VideoToVideoDto(video);
    }


    public async Task<List<VideoDTO>> GetRecommendedVideos(Guid userId,VideoCategory category,int topN )
    {
        var videoRecommendationsResponse = await _VideoRecommendationsRequestClient.GetResponse<RecommendationVideoResponse>(new
        {
            UserId = userId,
            Category = category,
            TopN = topN
        });
        var videos = await _videoRepository.GetVideosByVideoIds(videoRecommendationsResponse.Message.VideoIds);
        return videos.Select(_videoMapper.VideoToVideoDto).ToList();
    }
    
}