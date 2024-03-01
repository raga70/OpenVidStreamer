using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecommendationAlgo.Repository.Entities;

[Table("VideoLikes")]
public class VideoLike
{
    // [Key]
    // [Column(Order = 1)]
    public Guid VideoId { get; set; }

    // [Key]
    // [Column(Order = 2)]
    public Guid UserId { get; set; }


}