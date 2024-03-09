using Account.Model.DTO;
using Account.Services;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers;

[ApiController]
public class AccountController(AccountService _accountService):ControllerBase
{




    [HttpPost("register")]
    public async Task<OkObjectResult> Register([FromBody] RegisterRequestDTO request)
    {
        var accountNtoken = await _accountService.Register(request);
        
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
        var accId= Common.AccIdExtractorFromHttpContext.GetAccId(HttpContext);
        var balance = await _accountService.GetBalance(new Guid(accId));
        return Ok(balance);
    }


    [HttpPost("activateSubscription")]
    public async Task<ActionResult<AccountDTO>> ActivateSubscription()
    {
        var accId= Common.AccIdExtractorFromHttpContext.GetAccId(HttpContext);
        var account = await _accountService.ActivateSubscription(new Guid(accId));
        return Ok(account);
    }
   
}