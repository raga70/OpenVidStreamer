using Common.Model;

namespace RecommendationAlgo.Repository.Model.DTO;

public record VideoStatistics
{
    public Guid VideoId { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public decimal TotalWatchTime { get; set; }
    public int Views { get; set; }
    public VideoCategory Category { get; set; }
}