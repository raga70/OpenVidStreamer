using OpenVisStreamer.VideoLibrary.Repository.Entities;
using Riok.Mapperly.Abstractions;

namespace OpenVisStreamer.VideoLibrary.Model.Mappers;

[Mapper]
public partial class VideoMapper
{
    public partial VideoDTO VideoToVideoDto(Video video);
}