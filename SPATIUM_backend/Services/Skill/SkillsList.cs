using MediatR;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Services.Skill;

public class SkillsListQuery : IRequest<Result<List<string>>>
{
}

public class SkillsListQueryHandler : IRequestHandler<SkillsListQuery, Result<List<string>>>
{
	private readonly AppDbContext _appDbContext;

	public SkillsListQueryHandler(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}

	public async Task<Result<List<string>>> Handle(SkillsListQuery request, CancellationToken cancellationToken)
	{
		var skills = await _appDbContext.Skills.Select(x => x.Name).ToListAsync(cancellationToken);
		return Result<List<string>>.Success(skills);
	}
}