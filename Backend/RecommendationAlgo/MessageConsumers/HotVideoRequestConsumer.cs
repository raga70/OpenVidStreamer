using Common.MBcontracts;
using MassTransit;
using RecommendationAlgo.Repository;

namespace RecommendationAlgo.MessageConsumers;

public class HotVideoRequestConsumer(RecommendationRepository _repo) : IConsumer<HotVideoRequest>
{
    public async Task Consume(ConsumeContext<HotVideoRequest> context)
    {
        var VideoIds = await _repo.GetPopularVideos(context.Message.TopN);
      

        await context.RespondAsync<HotVideoResponse>(new
        {
           VideoIds = VideoIds 
        });
    }
}