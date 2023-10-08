using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Profile;

public class ProfileDetailsQuery : IRequest<Result<ProfileDto>>
{
	public string UserName { get; set; }
}

public class ProfileDetailsQueryHandler : IRequestHandler<ProfileDetailsQuery, Result<ProfileDto>>
{
	private readonly AppDbContext _appDbContext;
	private readonly MapperlyMapper _mapper;
	private readonly IUserAccessor _userAccessor;
	private readonly UserManager<Models.User> _userManager;

	public ProfileDetailsQueryHandler(AppDbContext appDbContext, MapperlyMapper mapper, IUserAccessor userAccessor, UserManager<Models.User> userManager)
	{
		_appDbContext = appDbContext;
		_mapper = mapper;
		_userAccessor = userAccessor;
		_userManager = userManager;
	}

	public async Task<Result<ProfileDto>> Handle(ProfileDetailsQuery request, CancellationToken cancellationToken)
	{
		var user = await _userManager.Users
			.AsQueryable()
			.Include(x => x.SkillUsers)
			.ThenInclude(x => x.Skill)
			.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

		if (user == null) return null;

		var profile = _mapper.MapCustom(user);
		return Result<ProfileDto>.Success(profile);
	}
}