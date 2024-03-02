using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Model;

namespace OpenVisStreamer.VideoLibrary.Repository.Entities;

[Table("Videos")] 
public record Video
{
    [Key] 
    public Guid VideoId { get; set; } = Guid.NewGuid(); 

    [Required] 
    [StringLength(255)] 
    public string Title { get; set; }

    [Required] 
    public string Description { get; set; }

    [EnumDataType(typeof(VideoCategory))] 
    public VideoCategory Category { get; set; }= VideoCategory.Other;

    [Column(TypeName = "decimal(18, 2)")] 
    public decimal videoLength { get; set; }
    
    [Required] 
    public string VideoUri { get; set; }

    [Required] 
    public string ThumbnailUri { get; set; }

   
    [Required]
    public Guid uploadedByAccoutId { get; set; }
    
   
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    
}
