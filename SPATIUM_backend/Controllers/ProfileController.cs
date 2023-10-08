using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Services.Profile;

namespace SPATIUM_backend.Controllers;

public class ProfileController : BaseApiController
{
	[AllowAnonymous]
	[HttpGet("{username}")]
	public async Task<IActionResult> GetProfile(string username)
	{
		return HandleResult(await Mediator.Send(new ProfileDetailsQuery { UserName = username }));
	}

	[HttpPut]
	public async Task<IActionResult> EditProfile(ProfileDto profile)
	{
		return HandleResult(await Mediator.Send(new EditProfileCommand { ProfileDto = profile }));
	}

	[HttpPatch]
	public async Task<IActionResult> EditProfile(JsonPatchDocument<ProfileDto> patchDocument)
	{
		return HandleResult(await Mediator.Send(new EditProfilePartialCommand { PatchDocument = patchDocument, ModelState = ModelState}));
	}
}