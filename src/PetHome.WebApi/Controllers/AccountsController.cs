using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Accounts.GetCurrentUser;
using PetHome.Application.Accounts.Login;
using PetHome.Application.Accounts.Register;
using PetHome.Application.Interfaces;
using Profile = PetHome.Application.Accounts.Profile;

namespace PetHome.WebApi.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IUserAccessor _user;
    public AccountController(ISender sender, IUserAccessor user)
    {
        _sender = sender;
        _user = user;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginCommand.LoginCommandRequest(request);
        var result =  await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized();
    }
    

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> Register(
        [FromForm] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new RegisterCommand.RegisterCommandRequest(request);
        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized();
    }
    
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> Me(CancellationToken cancellationToken)
    {
        var email = _user.GetEmail();
        var request = new GetCurrentUserRequest {Email = email};
        var query = new GetCurrentUserQuery.GetCurrentUserQueryRequest(request);
        var result =  await _sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized();
    }
}
