using Account.Model.DTO;
using Account.Services;
using Microsoft.AspNetCore.Mvc;
using OpenVisStreamer.VideoLibrary.Exceptions;

namespace Account.Controllers;

[ApiController]
public class AccountController(AccountService _accountService):ControllerBase
{




    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        Tuple<AccountDTO, string> accountNtoken;
        try
        {
         accountNtoken = await _accountService.Register(request);

        }
        catch (EmailAlreadyInUseException e)
        {
            return StatusCode(442);
        }
        
        return Ok(accountNtoken);
    }

    [HttpPost("login")]
    public async Task<OkObjectResult> Login([FromBody] LoginRequestDTO request)
    {
        var accountNtoken = await _accountService.Login(request);
        return Ok(accountNtoken);
    }

    [HttpGet("balance")]
    public async Task<ActionResult<decimal>> GetBalance()
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
        var balance = await _accountService.GetBalance(new Guid(accId));
        return Ok(balance);
    }


    [HttpPost("activateSubscription")]
    public async Task<ActionResult<AccountDTO>> ActivateSubscription()
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
        var account = await _accountService.ActivateSubscription(new Guid(accId));
        return Ok(account);
    }


    [HttpGet("getAccount")]
    public async Task<ActionResult<AccountDTO>> getAccount()
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
        var account = await _accountService.GetAccountById(new Guid(accId));
        return account;
    }
    
   
}