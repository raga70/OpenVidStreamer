using Account.Model.DTO;
using Account.Services;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers;

[ApiController]
public class AccountController(AccountService _accountService):ControllerBase
{




    [HttpPost("register")]
    public async Task<ActionResult<AccountDTO>> Register([FromBody] RegisterRequestDTO request)
    {
        var account = await _accountService.Register(request);
        return Ok(account);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccountDTO>> Login([FromBody] LoginRequestDTO request)
    {
        var account = await _accountService.Login(request);
        return Ok(account);
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