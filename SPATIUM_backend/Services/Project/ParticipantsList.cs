using MediatR;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Project;

public class ParticipantsListQuery : IRequest<Result<List<ParticipantDto>>>
{
	public Guid ProjectId { get; set; }
}

public class ParticipantsListQueryHandler : IRequestHandler<ParticipantsListQuery, Result<List<ParticipantDto>>>
{
	private readonly AppDbContext _appDbContext;

	public ParticipantsListQueryHandler(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}

	public async Task<Result<List<ParticipantDto>>> Handle(ParticipantsListQuery request, CancellationToken cancellationToken)
	{
		var project = await _appDbContext.Projects
			.AsQueryable()
			.Include(x => x.UserProjects)
			.ThenInclude(x => x.User)
			.FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

		if (project == null) return null;

		var participants = project.UserProjects
			.Where(x => x.IsHost == false)
			.Select(x => new ParticipantDto { UserName = x.User.UserName, IsAccepted = x.IsAccepted })
			.ToList();

		return Result<List< ParticipantDto>>.Success(participants);
	}
}