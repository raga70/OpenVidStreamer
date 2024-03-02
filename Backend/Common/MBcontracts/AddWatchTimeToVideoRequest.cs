namespace Common.MBcontracts;

public record AddWatchTimeToVideoRequest
{
   public Guid UserId { get; set; }
   public  Guid VideoId { get; set; }
   public decimal WatchedTime { get; set; }
}