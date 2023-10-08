using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Extensions;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Project;

public class CreateProjectCommand : IRequest<Result<Unit>>
{
	public ProjectDto Project { get; set; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Unit>>
{
	private readonly AppDbContext _appDbContext;
	private readonly MapperlyMapper _mapper;
	private readonly IUserAccessor _userAccessor;
	private readonly UserManager<Models.User> _userManager;

	public CreateProjectCommandHandler(AppDbContext appDbContext, MapperlyMapper mapper, IUserAccessor userAccessor,
		UserManager<Models.User> userManager)
	{
		_appDbContext = appDbContext;
		_mapper = mapper;
		_userAccessor = userAccessor;
		_userManager = userManager;
	}

	public async Task<Result<Unit>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
	{
		var user = _userManager.Users.FirstOrDefault(x => x.UserName == _userAccessor.GetUsername());

		var project = _mapper.Map(request.Project);

		await project.RequiredSkills.MapToSkillProjectAsync(request.Project.RequiredSkills, project, _appDbContext,
			cancellationToken);

		project.Creator = user;

		var userProject = new UserProject
		{
			IsHost = true,
			Project = project,
			ProjectId = project.Id,
			User = user,
			UserId = user!.Id,
		};

		project.UserProjects.Add(userProject);

		project.CreationDate = DateTime.Now;

		await _appDbContext.AddAsync(project, cancellationToken);

		var result = await _appDbContext.SaveChangesAsync(cancellationToken) > 0;

		return !result ? Result<Unit>.Failure("Не вдалося створити проект") : Result<Unit>.Success(Unit.Value);
	}
}