using Common.MBcontracts;
using MassTransit;
using Render.Services;

namespace Render.MessageConsumers;

public sealed class RenderVideoRequestConsumer(RenderService renderService) :IConsumer<RenderVideoRequest>
{
    public async Task Consume(ConsumeContext<RenderVideoRequest> context)
    {
       await renderService.RenderVideo(context.Message.VideoId, context.Message.VideoUri);
    }
}