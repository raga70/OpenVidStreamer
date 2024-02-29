namespace OpenVisStreamer.VideoLibrary.Model.Entities;

public class VideoLike
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public Video Video { get; set; }
}