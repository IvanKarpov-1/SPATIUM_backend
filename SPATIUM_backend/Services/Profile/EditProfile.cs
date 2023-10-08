using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Extensions;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Profile;

public class EditProfileCommand : IRequest<Result<Unit>>
{
	public ProfileDto ProfileDto { get; set; } 
}

public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Result<Unit>>
{
	private readonly AppDbContext _appDbContext;
	private readonly MapperlyMapper _mapper;
	private readonly IUserAccessor _userAccessor;

	public EditProfileCommandHandler(AppDbContext appDbContext, MapperlyMapper mapper, IUserAccessor userAccessor)
	{
		_appDbContext = appDbContext;
		_mapper = mapper;
		_userAccessor = userAccessor;
	}

	public async Task<Result<Unit>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
	{
		var user = await _appDbContext.Users
			.AsQueryable()
			.Include(x => x.SkillUsers)
			.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(),
			cancellationToken);

		if (user == null) return null;

		_mapper.Map(request.ProfileDto, user);

		await user.SkillUsers.MapToSkillUsersAsync(request.ProfileDto.Skills, user, _appDbContext, cancellationToken);

		var result = await _appDbContext.SaveChangesAsync(cancellationToken) > 0;

		return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Не вдалося оновити профіль користувача");
	}
}