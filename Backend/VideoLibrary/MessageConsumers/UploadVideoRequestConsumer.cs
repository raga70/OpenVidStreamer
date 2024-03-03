using Common.MBcontracts;
using MassTransit;
using OpenVisStreamer.VideoLibrary.Model.Mappers;
using OpenVisStreamer.VideoLibrary.Repository;

namespace OpenVisStreamer.VideoLibrary.MessageConsumers;

public class UploadVideoRequestConsumer(VideoRepository _repo) : IConsumer<UploadVideoRequest>
{
    public async Task Consume(ConsumeContext<UploadVideoRequest> context)
    {
        var video = UploadVideoMapper.UploadVideoRequestToVideo(context.Message);
        await _repo.Create(video);
    }
}