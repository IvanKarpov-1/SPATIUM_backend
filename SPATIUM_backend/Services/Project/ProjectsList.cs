using MediatR;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Core;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Project;

public class ProjectsListQuery : IRequest<Result<List<ProjectDto>>>
{
}

public class ProjectsListQueryHandler : IRequestHandler<ProjectsListQuery, Result<List<ProjectDto>>>
{
	private readonly AppDbContext _appDbContext;
	private readonly MapperlyMapper _mapper;

	public ProjectsListQueryHandler(AppDbContext appDbContext, MapperlyMapper mapper)
	{
		_appDbContext = appDbContext;
		_mapper = mapper;
	}

	public async Task<Result<List<ProjectDto>>> Handle(ProjectsListQuery request, CancellationToken cancellationToken)
	{
		var projects = await _appDbContext.Projects
			.AsQueryable()
			.Include(x => x.Creator)
			.Include(x => x.RequiredSkills)
			.ThenInclude(x => x.Skill)
			.ToListAsync(cancellationToken);

		var projectsDto = projects.Select(_mapper.MapCustom).ToList();

		return Result<List<ProjectDto>>.Success(projectsDto);
	}
}