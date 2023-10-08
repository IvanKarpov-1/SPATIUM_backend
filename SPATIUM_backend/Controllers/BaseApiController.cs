using MediatR;
using Microsoft.AspNetCore.Mvc;
using SPATIUM_backend.Models;

namespace SPATIUM_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
	private IMediator _mediator;

	protected IMediator Mediator => _mediator ??=
		HttpContext.RequestServices.GetService<IMediator>();

	protected ActionResult HandleResult<T>(Result<T> result)
	{
		if (result == null) return NotFound();
		switch (result.IsSuccess)
		{
			case true when result.Value != null:
				return Ok(result.Value);
			case true when result.Value == null:
				return NotFound();
			case false when result.Value == null && result.Error == string.Empty:
				return Unauthorized();
			case false when result.ValidationError:
				if (result.Error != null) ModelState.AddModelError("error", result.Error);
				return ValidationProblem(ModelState);
			default:
				return BadRequest();
		}
	}
}