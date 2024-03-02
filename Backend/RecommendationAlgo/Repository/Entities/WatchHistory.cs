using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Model;

namespace RecommendationAlgo.Repository.Entities;

public class WatchHistory
{
    [Key]
    [Column(Order = 0)]
    public Guid UserId { get; set; }
    
    [Key]
    [Column(Order = 1)]
    public Guid VideoId { get; set; }
    
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal WatchedTime { get; set; } = 0;
    
    
    public VideoLikeEnum Liked { get; set; } = VideoLikeEnum.NotRated;
    

    public bool FullyWatched { get; set; } = false;
    
}