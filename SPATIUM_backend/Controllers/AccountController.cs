using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Services.User;

namespace SPATIUM_backend.Controllers;

public class AccountController : BaseApiController
{
	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginDto loginDto)
	{
		return HandleResult(await Mediator.Send(new LoginCommand { LoginDto = loginDto }));
	}

	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterDto registerDto)
	{
		return HandleResult(await Mediator.Send(new RegisterCommand { RegisterDto = registerDto }));
	}

	[HttpGet]
	public async Task<IActionResult> GetCurrentUser()
	{
		return HandleResult(await Mediator.Send(new UserDetailsQuery { User = User }));
	}
}