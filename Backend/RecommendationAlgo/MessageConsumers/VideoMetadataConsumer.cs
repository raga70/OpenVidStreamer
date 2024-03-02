using Common.MBcontracts;
using MassTransit;
using RecommendationAlgo.Repository;

namespace RecommendationAlgo.MessageConsumers;

public sealed class VideoMetadataConsumer(RecommendationRepository _repo) :IConsumer<VideoMetadataPopulationRequest>
{
    public async Task Consume(ConsumeContext<VideoMetadataPopulationRequest> context)
    {
      await  _repo.PopulateVideoMetadata(context.Message, context.CancellationToken);
    }
}