namespace Common.MBcontracts;

public record UpdateVideoToPublicRequest
{
  public  Guid VideoId { get; set; }
  public decimal VideoLength { get; set; }
}