using MediatR;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Project;

public class ToggleParticipantCommand : IRequest<Result<Unit>>
{
	public Guid ProjectId { get; set; }
	public string UserName { get; set; }
}

public class ToggleParticipantCommandHandler : IRequestHandler<ToggleParticipantCommand, Result<Unit>>
{
	private readonly AppDbContext _appDbContext;
	private readonly IUserAccessor _userAccessor;

	public ToggleParticipantCommandHandler(AppDbContext appDbContext, IUserAccessor userAccessor)
	{
		_appDbContext = appDbContext;
		_userAccessor = userAccessor;
	}

	public async Task<Result<Unit>> Handle(ToggleParticipantCommand request, CancellationToken cancellationToken)
	{
		var project = await _appDbContext.Projects
			.AsQueryable()
			.Include(x => x.UserProjects)
			.ThenInclude(x => x.User)
			.Include(x => x.Creator)
			.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

		var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

		if (project == null) return null;
		if (user == null) return null;
		if (project.Creator.Id != user.Id) return Result<Unit>.Failure("Не можна змінювати статус людей в чужих проектах").IsValidationError();

		var userProject = project.UserProjects.FirstOrDefault(x => x.User.UserName == request.UserName);

		if (userProject == null) return null;

		if (userProject.IsHost) return Result<Unit>.Failure("Не можна змінити свій статус, оскільки ви творець проекту");

		userProject.IsAccepted = !userProject.IsAccepted;

		var result = await _appDbContext.SaveChangesAsync(cancellationToken) > 0;

		return !result ? Result<Unit>.Failure($"Не вдалося оновити статус {request.UserName}") : Result<Unit>.Success(Unit.Value);
	}
}