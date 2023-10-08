using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPATIUM_backend.Controllers;

public class TestController : BaseApiController
{
	[AllowAnonymous]
	[HttpGet("echo/{text}")]
	public async Task<IActionResult> Test(string text)
	{
		return Ok(text);
	}
}