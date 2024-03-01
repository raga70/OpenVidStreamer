using Common.MBcontracts;
using MassTransit;

namespace RecommendationAlgo.Messaging;

public class VideoRecommendationsRequestConsumer
{
    public async Task Consume(ConsumeContext<RecommendationVideoRequest> context)
    {
        var userId = context.Message.UserId;
        var category = context.Message.Category;

        var recommendedVideoIds = new List<Guid>(); //Todo: call the repo and get video recommendations based on user id

        await context.RespondAsync<RecommendationVideoResponse>(new
        {
            UserId = userId,
            VideoIds = recommendedVideoIds
        });
    }
}