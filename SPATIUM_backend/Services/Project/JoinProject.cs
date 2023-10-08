using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Project;

public class JoinProjectCommand : IRequest<Result<Unit>>
{
	public Guid ProjectId { get; set; }
}

public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand, Result<Unit>>
{
	private readonly AppDbContext _appDbContext;
	private readonly MapperlyMapper _mapper;
	private readonly IUserAccessor _userAccessor;
	private readonly UserManager<Models.User> _userManager;

	public JoinProjectCommandHandler(AppDbContext appDbContext, MapperlyMapper mapper, IUserAccessor userAccessor, UserManager<Models.User> userManager)
	{
		_appDbContext = appDbContext;
		_mapper = mapper;
		_userAccessor = userAccessor;
		_userManager = userManager;
	}

	public async Task<Result<Unit>> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
	{
		var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

		if (user == null) return null;

		var project = await _appDbContext.Projects
			.AsQueryable()
			.Include(x => x.UserProjects)
			.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

		if (project == null) return null;

		//if (project.UserProjects.Any(x => x.IsHost && x.UserId == user.Id)) return Result<Unit>.Failure("Не можна долужичитись до свого проекту");

		foreach (var projectUserProject in project.UserProjects)
		{
			if (projectUserProject.IsHost && projectUserProject.UserId == user.Id)
			{
				return Result<Unit>.Failure("Не можна долучитись до свого проекту").IsValidationError();
			}
		}

		var userProject = new UserProject
		{
			IsHost = false,
			IsAccepted = false,
			Project = project,
			ProjectId = project.Id,
			User = user,
			UserId = user.Id
		};

		project.UserProjects.Add(userProject);

		var result = await _appDbContext.SaveChangesAsync(cancellationToken) > 0;

		return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Не вдалося долучитись до проекту");
	}
}