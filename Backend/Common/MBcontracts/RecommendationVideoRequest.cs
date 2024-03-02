using Common.Model;

namespace Common.MBcontracts;

public record RecommendationVideoRequest
{
   public Guid UserId { get; set; }
   public VideoCategory? Category { get; set; }
   public int TopN { get; set; }
}