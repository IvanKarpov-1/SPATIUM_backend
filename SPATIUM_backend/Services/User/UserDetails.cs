using MediatR;
using Microsoft.AspNetCore.Identity;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;
using System.Security.Claims;

namespace SPATIUM_backend.Services.User;

public class UserDetailsQuery : IRequest<Result<UserDto>>
{
	public ClaimsPrincipal User { get; set; }
}

public class UserDetailsQueryHandler : IRequestHandler<UserDetailsQuery, Result<UserDto>>
{
	private readonly UserManager<Models.User> _userManager;
	private readonly ITokenService _tokenService;

	public UserDetailsQueryHandler(UserManager<Models.User> userManager, ITokenService tokenService)
	{
		_userManager = userManager;
		_tokenService = tokenService;
	}

	public async Task<Result<UserDto>> Handle(UserDetailsQuery request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByEmailAsync(request.User.FindFirstValue(ClaimTypes.Email)!);

		return Result<UserDto>.Success(new UserDto
		{
			Token = _tokenService.CreateToken(user),
			UserName = user.UserName,
		});
	}
}