using Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace Upload.Model.DTO;

public class VideoUploadDTO : ControllerBase
{
  
    public string Title { get; set; }

 
    public string Description { get; set; }

   
    public VideoCategory Category { get; set; }= VideoCategory.Other;
    
    public string ThumbnailUri { get; set; }
}