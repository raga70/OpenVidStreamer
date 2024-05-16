using Common.Model;
using Microsoft.AspNetCore.Mvc;
using OpenVisStreamer.VideoLibrary.Model;
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
    public async Task<ActionResult<List<VideoDTO>>> GetRecommendedVideos([FromQuery]VideoCategory category,[FromQuery]int topN = 20)
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
        
        var videos = await _videoService.GetRecommendedVideos(new Guid(accId),category,topN);
        return Ok(videos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<VideoDTO>> GetVideoById(Guid id)
    {
        var video = await _videoService.GetVideoById(id);
        if (video is null)
        {
            return NotFound();
        }
        return Ok(video);
    }

    
    [HttpGet("hotVideos")]
    public async Task<ActionResult<List<VideoDTO>>> GetHotVideos([FromQuery]int topN = 20)
    {
        var videos = await _videoService.GetHotVideos(topN);
        
        
        return Ok(videos);
    }
    
    
    
    [HttpGet("searchVideos")]
    public async Task<ActionResult<List<VideoDTO>> > FindVideosByTittle([FromQuery]string searchQuery)
    {
        var videos = await _videoService.FindVideosByTittle(searchQuery);
        return Ok(videos);
    }
    
    
    
}