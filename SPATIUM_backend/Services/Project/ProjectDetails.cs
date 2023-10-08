using MediatR;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Project;

public class ProjectDetailsQuery : IRequest<Result<ProjectDto>>
{
	public Guid Id { get; set; }
}

public class ProjectDetailsQueryHandler : IRequestHandler<ProjectDetailsQuery, Result<ProjectDto>>
{
	private readonly AppDbContext _appDbContext;
	private readonly MapperlyMapper _mapper;

	public ProjectDetailsQueryHandler(AppDbContext appDbContext, MapperlyMapper mapper)
	{
		_appDbContext = appDbContext;
		_mapper = mapper;
	}

	public async Task<Result<ProjectDto>> Handle(ProjectDetailsQuery request, CancellationToken cancellationToken)
	{
		var project = await _appDbContext.Projects
			.AsQueryable()
			.Include(x => x.Creator)
			.Include(x => x.RequiredSkills)
			.ThenInclude(x => x.Skill)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		return project == null ? null : Result<ProjectDto>.Success(_mapper.MapCustom(project));
	}
}