using OpenVisStreamer.VideoLibrary.Model.Entities;

namespace Common.MBcontracts;

public record RecommendationVideoRequest
{
   public Guid UserId { get; }
   public VideoCategory? Category { get; }
}