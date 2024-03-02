using Common.MBcontracts;
using Common.Model;
using MassTransit;
using RecommendationAlgo.Repository;

namespace RecommendationAlgo.MessageConsumers;

public sealed class VideoRecommendationsRequestConsumer(RecommendationRepository _repo) : IConsumer<RecommendationVideoRequest>
{
    public async Task Consume(ConsumeContext<RecommendationVideoRequest> context) 
    {
        var userId = context.Message.UserId;
        var category = context.Message.Category;
        var topN = context.Message.TopN;

        List<Guid> recommendedVideoIds = new();
        if (category is null or VideoCategory.Other)
            recommendedVideoIds = await _repo.GetAlgoRecommendedVideos(userId, topN);
        else
            recommendedVideoIds = await _repo.GetAlgoRecommendedVideosForCategory(userId,topN,category.Value);
          
        
        
        await context.RespondAsync<RecommendationVideoResponse>(new
        {
            UserId = userId,
            VideoIds = recommendedVideoIds
        });
    }
}