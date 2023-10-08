using Microsoft.AspNetCore.Identity;

namespace SPATIUM_backend.Models;

public class User : IdentityUser
{
	public ICollection<SkillUser> SkillUsers { get; set; } = new List<SkillUser>();
	public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;
	public string ImageUrl { get; set; }
}