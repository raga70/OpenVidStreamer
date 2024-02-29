using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OpenVisStreamer.VideoLibrary.Model.Entities;

[Table("Videos")] 
public record Video
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid(); 

    [Required] 
    [StringLength(255)] 
    public string Title { get; set; }

    [Required] 
    public string Description { get; set; }

    [EnumDataType(typeof(VideoCategory))] 
    public VideoCategory Category { get; set; }

    [Required] 
    public string VideoUri { get; set; }

    [Required] 
    public string ThumbnailUri { get; set; }

    public List<Guid> LikedBy { get; set; } = new List<Guid>();

    public List<Guid> DislikedBy { get; set; } = new List<Guid>();

    [Column(TypeName = "decimal(18, 2)")] 
    public decimal WatchTime { get; set; }

  
}
