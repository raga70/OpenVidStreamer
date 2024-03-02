using Common.MBcontracts;
using MassTransit;
using RecommendationAlgo.Repository;

namespace RecommendationAlgo.MessageConsumers;

public sealed class DeleteVideoRequestConsumer(RecommendationRepository _repo) : IConsumer<DeleteVideoRequest>
{
    public async Task Consume(ConsumeContext<DeleteVideoRequest> context)
    {
        await _repo.DeleteVideo(context.Message.VideoId);
    }
}
