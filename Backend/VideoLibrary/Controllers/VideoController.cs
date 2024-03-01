using Microsoft.AspNetCore.Mvc;
using OpenVisStreamer.VideoLibrary.Model;
using OpenVisStreamer.VideoLibrary.Model.Entities;
using OpenVisStreamer.VideoLibrary.Services;

namespace OpenVisStreamer.VideoLibrary.Controllers;

[ApiController]
public class VideoController(VideoService _videoService) : ControllerBase
{
 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="category">if it`s not provided it will give general recommendations</param>
    /// <returns></returns>
    [HttpGet("recommendedVideos")]
    public async Task<ActionResult<List<VideoDTO>>> GetRecommendedVideos(VideoCategory category)
    {
        var userId = Common.AccIdExtractorFromHttpContext.GetAccId(HttpContext);
        
        var videos = await _videoService.GetRecommendedVideos(new Guid(userId),category);
        return Ok(videos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<VideoDTO>> GetVideoById(Guid id)
    {
        var video = await _videoService.GetVideoById(id);
        if (video == null)
        {
            return NotFound();
        }
        return Ok(video);
    }

    
    
    
    
}