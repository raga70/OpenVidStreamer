using Common.Model;

namespace Common.MBcontracts;

public class VideoMetadataPopulationRequest
{
   public Guid VideoId { get; set; }
   public decimal VideoLength { get; set; }
   public VideoCategory Category { get; set; }
}