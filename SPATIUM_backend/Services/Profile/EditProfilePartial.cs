using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Extensions;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Profile;

public class EditProfilePartialCommand : IRequest<Result<Unit>>
{
	public JsonPatchDocument<ProfileDto> PatchDocument { get; set; }
	public ModelStateDictionary ModelState { get; set; }
}

public class EditProfilePartialCommandHandler : IRequestHandler<EditProfilePartialCommand, Result<Unit>>
{
	private readonly AppDbContext _appDbContext;
	private readonly IUserAccessor _userAccessor;
	private readonly MapperlyMapper _mapper;

	public EditProfilePartialCommandHandler(AppDbContext appDbContext, IUserAccessor userAccessor, MapperlyMapper mapper)
	{
		_appDbContext = appDbContext;
		_userAccessor = userAccessor;
		_mapper = mapper;
	}

	public async Task<Result<Unit>> Handle(EditProfilePartialCommand request, CancellationToken cancellationToken)
	{
		if (request.PatchDocument == null) return Result<Unit>.Failure("");

		var user = await _appDbContext.Users
			.AsQueryable()
			.Include(x => x.SkillUsers)
			.ThenInclude(x => x.Skill)
			.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(),
			cancellationToken);

		if (user == null) return null;

		var profileDto = _mapper.MapCustom(user);

		request.PatchDocument.ApplyTo(profileDto, request.ModelState);

		if (request.ModelState.IsValid == false) return Result<Unit>.Failure("").IsValidationError();

		_mapper.Map(profileDto, user);
		
		await user.SkillUsers.MapToSkillUsersAsync(profileDto.Skills, user, _appDbContext, cancellationToken);

		var result = await _appDbContext.SaveChangesAsync(cancellationToken) > 0;

		return !result ? Result<Unit>.Failure("Не вдалося оновити проект") : Result<Unit>.Success(Unit.Value);
	}
}