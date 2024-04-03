using Common.MBcontracts;
using MassTransit;
using OpenVisStreamer.VideoLibrary.Repository;

namespace OpenVisStreamer.VideoLibrary.MessageConsumers;

public class UpdateVideoToPublicRequestConsumer(VideoRepository _repo, IBus bus) : IConsumer<UpdateVideoToPublicRequest>
{
    public async Task Consume(ConsumeContext<UpdateVideoToPublicRequest> context)
    {
       var video = await _repo.UpdateVideoToPublic(context.Message.VideoId, context.Message.VideoLength);

       await bus.Publish<VideoMetadataPopulationRequest>(new
       {
           VideoId = video.VideoId, VideoLength = video.videoLength, Category = video.Category
       });
    }
}