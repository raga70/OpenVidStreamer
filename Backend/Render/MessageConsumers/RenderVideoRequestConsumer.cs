using Common.MBcontracts;
using MassTransit;
using Render.Services;

namespace Render.MessageConsumers;

public sealed class RenderVideoRequestConsumer(RenderService renderService) :IConsumer<UploadVideoRequest>
{
    public async Task Consume(ConsumeContext<UploadVideoRequest> context)
    {
       await renderService.RenderVideo(context.Message.VideoId, context.Message.VideoUri);
    }
}