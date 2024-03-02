using Common.MBcontracts;
using MassTransit;
using RecommendationAlgo.Repository;

namespace RecommendationAlgo.MessageConsumers;

public sealed class AddWatchTimeToVideoRequestConsumer(RecommendationRepository _repo) : IConsumer<AddWatchTimeToVideoRequest>
{
    public async Task Consume(ConsumeContext<AddWatchTimeToVideoRequest> context)
    {
      await _repo.AddWatchTime(context.Message.UserId, context.Message.VideoId, context.Message.WatchedTime);
    }
}

