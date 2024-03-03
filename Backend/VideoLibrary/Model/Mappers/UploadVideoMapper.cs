using Common.MBcontracts;
using OpenVisStreamer.VideoLibrary.Repository.Entities;
using Riok.Mapperly.Abstractions;

namespace OpenVisStreamer.VideoLibrary.Model.Mappers;

[Mapper]
public  static  partial class UploadVideoMapper
{
    public static partial Video UploadVideoRequestToVideo(UploadVideoRequest uploadVideoRequest);
}