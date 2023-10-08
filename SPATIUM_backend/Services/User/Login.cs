using MediatR;
using Microsoft.AspNetCore.Identity;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.User;

public class LoginCommand : IRequest<Result<UserDto>>
{
	public LoginDto LoginDto { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserDto>>
{
	private readonly UserManager<Models.User> _userManager;
	private readonly ITokenService _tokenService;

	public LoginCommandHandler(UserManager<Models.User> userManager, ITokenService tokenService)
	{
		_userManager = userManager;
		_tokenService = tokenService;
	}

	public async Task<Result<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByEmailAsync(request.LoginDto.Email);

		if (user == null) return Result<UserDto>.Failure(string.Empty);

		var result = await _userManager.CheckPasswordAsync(user, request.LoginDto.Password);

		if (!result) return Result<UserDto>.Failure(string.Empty);

		return Result<UserDto>.Success(new UserDto
		{
			Token = _tokenService.CreateToken(user),
			UserName = user.UserName,
		});
	}
}