using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Model;

namespace RecommendationAlgo.Repository.Entities;

public class VideoStats 
{
    [Key]
    public Guid VideoId { get; set; }
    
    [Required]
    public decimal VideoLength { get; set; }
    [Required]
    public VideoCategory Category { get; set; } = VideoCategory.Other;
    
    [Required]
    public DateTime PublishedAt { get; set; } = DateTime.Now;
    
    
}