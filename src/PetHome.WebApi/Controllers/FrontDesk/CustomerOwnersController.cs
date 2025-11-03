using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Accounts;
using PetHome.Application.Accounts.RegisterAsOwner;
using PetHome.Application.Interfaces;

namespace PetHome.WebApi.Controllers.FrontDesk;

[ApiController]
[Route("api/customer")]
public class CustomerOwnersController : ControllerBase
{
	private readonly ISender _sender;
	private readonly IConfiguration _configuration;
	private readonly IUserAccessor _user;
	public CustomerOwnersController(ISender sender, IUserAccessor user, IConfiguration configuration)
	{
		_sender = sender;
		_user = user;
		_configuration = configuration;
	}

	// [AllowAnonymous]
	// [HttpPost("login")]
	// [ProducesResponseType((int)HttpStatusCode.OK)]
	// public async Task<ActionResult<Profile>> Login(
	// 	[FromBody] LoginRequest request,
	// 	CancellationToken cancellationToken
	// )
	// {
	// 	var command = new LoginCommand.LoginCommandRequest(request);
	// 	var result = await _sender.Send(command, cancellationToken);
	// 	return result.IsSuccess ? Ok(result.Value) : Unauthorized();
	// }


	[AllowAnonymous]
	[HttpPost("register")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<ActionResult<Profile>> Register(
		[FromForm] RegisterAsOwnerRequest request,
		CancellationToken cancellationToken
	)
	{
		var callbackBaseUrl = $"{_configuration["Frontend:BaseUrl"]}/set-password";
		var command = new RegisterAsOwnerCommand.RegisterOwnerCommandRequest(request, callbackBaseUrl!);
		var result = await _sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : Unauthorized();
	}
}