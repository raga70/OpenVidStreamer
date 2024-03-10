using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Upload.Model.DTO;
using Upload.Services;

namespace Upload.Controllers;

[ApiController]
public class UploadController(UploadService uploadService) :ControllerBase
{
     /// <summary>
    /// 
    /// </summary>
    /// <param name="videoMetadata"> object of type <see cref="VideoUploadDTO"/></param>
    /// <param name="videoFile"> the videoFile</param>
    /// <param name="thumbnailFile"> the thumbnailFile </param>
    /// <returns></returns>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideoWithMetadata(
       [FromForm] string videoMetadata, 
       [FromForm]  IFormFile videoFile, 
       [FromForm]   IFormFile thumbnailFile)
    {

       // var accId = AccIdExtractorFromHttpContext.GetAccId(HttpContext);
       var accId = Guid.NewGuid().ToString();//todo: remove this line and uncomment the line above (used for testing without auth and api gateway)
        
        if (videoFile == null || videoFile.Length == 0)
        {
            return BadRequest("video file upload failed");
        }
        if (thumbnailFile == null || thumbnailFile.Length == 0)
        {
            return BadRequest("thumbnail file upload failed");
        }

        // Deserialize the JSON metadata to the VideoDTO object
        VideoUploadDTO videoDTO = JsonConvert.DeserializeObject<VideoUploadDTO>(videoMetadata);
        if (videoDTO == null)
        {
            return BadRequest("Invalid video metadata.");
        }
        
        await uploadService.UploadVideo(videoDTO, videoFile, thumbnailFile, accId);
        return Ok();
    }
}
