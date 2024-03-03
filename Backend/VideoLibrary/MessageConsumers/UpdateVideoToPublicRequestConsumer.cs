using Common.MBcontracts;
using MassTransit;
using OpenVisStreamer.VideoLibrary.Repository;

namespace OpenVisStreamer.VideoLibrary.MessageConsumers;

public class UpdateVideoToPublicRequestConsumer(VideoRepository _repo) : IConsumer<UpdateVideoToPublicRequest>
{
    public async Task Consume(ConsumeContext<UpdateVideoToPublicRequest> context)
    {
        await _repo.UpdateVideoToPublic(context.Message.VideoId);
    }
}