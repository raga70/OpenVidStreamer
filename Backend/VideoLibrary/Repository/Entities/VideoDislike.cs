namespace OpenVisStreamer.VideoLibrary.Model.Entities;

public class VideoDislike
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public Video Video { get; set; }
    public User User { get; set; }
}