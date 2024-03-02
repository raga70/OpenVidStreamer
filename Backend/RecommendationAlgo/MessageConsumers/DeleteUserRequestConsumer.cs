using Common.MBcontracts;
using MassTransit;
using RecommendationAlgo.Repository;

namespace RecommendationAlgo.MessageConsumers;

public sealed class DeleteUserRequestConsumer (RecommendationRepository _repo): IConsumer<DeleteUserRequest>
{
    public async Task Consume(ConsumeContext<DeleteUserRequest> context)
    {
       await _repo.DeleteUserData(context.Message.UserId);
    }
}