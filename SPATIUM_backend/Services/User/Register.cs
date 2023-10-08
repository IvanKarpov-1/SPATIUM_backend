using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.User;

public class RegisterCommand : IRequest<Result<UserDto>>
{
	public RegisterDto RegisterDto { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
{
	private readonly UserManager<Models.User> _userManager;
	private readonly ITokenService _tokenService;

	public RegisterCommandHandler(UserManager<Models.User> userManager, ITokenService tokenService)
	{
		_userManager = userManager;
		_tokenService = tokenService;
	}

	public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{
		if (await _userManager.Users.AnyAsync(x => x.UserName == request.RegisterDto.Username, cancellationToken))
		{
			return Result<UserDto>.Failure("Ім'я користувача вже зайняте").IsValidationError();
		}

		if (await _userManager.Users.AnyAsync(x => x.Email == request.RegisterDto.Email, cancellationToken))
		{
			return Result<UserDto>.Failure("Електронна пошта користувача вже зайнята").IsValidationError();
		}

		var user = new Models.User
		{
			Email = request.RegisterDto.Email,
			UserName = request.RegisterDto.Username,
			CreationDate = DateTime.Now
		};

		var result = await _userManager.CreateAsync(user, request.RegisterDto.Password);

		if (result.Succeeded)
		{
			return Result<UserDto>.Success(new UserDto
			{
				Token = _tokenService.CreateToken(user),
				UserName = user.UserName,
			});
		}

		return Result<UserDto>.Failure(result.Errors.ToString());
	}
}