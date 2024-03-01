namespace RecommendationAlgo.Repository.Entities;

public class WatchHistory
{
    public Guid UserId { get; set; }
    public Guid VideoId { get; set; }
    public decimal WatchedTime { get; set; }
    public bool Liked { get; set; }
    public bool FullWatched { get; set; }
    
}