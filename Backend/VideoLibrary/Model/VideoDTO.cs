using Common.Model;

namespace OpenVisStreamer.VideoLibrary.Model;
public record VideoDTO
{
    public Guid VideoId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public VideoCategory Category { get; set; }
    public decimal VideoLength { get; set; }
    public string VideoUri { get; set; }
    public string ThumbnailUri { get; set; }
    public int TotalLikes { get; set; }
    public int TotalDislikes { get; set; }
    public DateTime UploadDateTime { get; set; }
};