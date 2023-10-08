using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPATIUM_backend.Services.Skill;

namespace SPATIUM_backend.Controllers;

public class SkillController : BaseApiController
{
	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return HandleResult(await Mediator.Send(new SkillsListQuery()));
	}
}