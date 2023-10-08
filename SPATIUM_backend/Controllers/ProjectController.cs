using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Services.Project;

namespace SPATIUM_backend.Controllers;

public class ProjectController : BaseApiController
{
	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return HandleResult(await Mediator.Send(new ProjectsListQuery()));
	}

	[AllowAnonymous]
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Get(Guid id)
	{
		return HandleResult(await Mediator.Send(new ProjectDetailsQuery { Id = id }));
	}

	[HttpPost]
	public async Task<IActionResult> CreateProject(ProjectDto project)
	{
		return HandleResult(await Mediator.Send(new CreateProjectCommand { Project = project }));
	}

	[HttpPost("join/{projectId:guid}")]
	public async Task<IActionResult> Join(Guid projectId)
	{
		return HandleResult(await Mediator.Send(new JoinProjectCommand { ProjectId = projectId }));
	}

	[AllowAnonymous]
	[HttpGet("participants/{projectId:guid}")]
	public async Task<IActionResult> GetParticipants(Guid projectId)
	{
		return HandleResult(await Mediator.Send(new ParticipantsListQuery { ProjectId = projectId }));
	}

	[HttpPost("toggle")]
	public async Task<IActionResult> ToggleStatus(ToggleParticipantCommand command)
	{
		return HandleResult(await Mediator.Send(command));
	}
}