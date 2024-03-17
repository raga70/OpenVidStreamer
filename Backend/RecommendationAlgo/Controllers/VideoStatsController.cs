using Microsoft.AspNetCore.Mvc;
using RecommendationAlgo.Repository;
using RecommendationAlgo.Repository.Entities;

namespace RecommendationAlgo.Controllers;

[ApiController]
public class VideoStatsController(RecommendationRepository _repo) : ControllerBase
{
    [HttpGet("getVideoStatistics")]
    public async Task<ActionResult<VideoStats>> GetVideoStats([FromBody]  Guid videoId)
    {
        return Ok(await _repo.GetVideoStatistics(videoId));
    }
    
    [HttpPost("likeVideo")]
    public async Task<ActionResult> LikeVideo([FromBody]  Guid videoId)
    {
       HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
        await _repo.LikeVideo(new Guid(accId), videoId);
        return Ok();
    }

    [HttpPost("dislikeVideo")]
    public async Task<ActionResult> DislikeVideo([FromBody]  Guid videoId)
    {
     
        HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
       await _repo.DislikeVideo(new Guid(accId), videoId);
        return Ok();
    }
    
    [HttpGet("preferedCategory")]
    public async Task<ActionResult<List<VideoStats>>> GetPreferedCategory([FromQuery] int topN)
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
        return Ok(await _repo.GetUserPreferredCategories(new Guid(accId),topN));
    }
    
    
    
        
    
    
    
}