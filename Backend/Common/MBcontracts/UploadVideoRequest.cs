using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Model;

namespace Common.MBcontracts;

public class UploadVideoRequest
{

    public Guid VideoId { get; set; } = Guid.NewGuid(); 

    public string Title { get; set; }

    public string Description { get; set; }

    public VideoCategory Category { get; set; }= VideoCategory.Other;

    public decimal videoLength { get; set; }
    
    public string VideoUri { get; set; }

    public string ThumbnailUri { get; set; }

   
    public Guid uploadedByAccoutId { get; set; }
    
    public DateTime UploadDateTime { get; set; } = DateTime.Now;
    
    public bool IsPublic { get; set; } = false;
}