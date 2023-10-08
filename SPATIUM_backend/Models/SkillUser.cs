namespace SPATIUM_backend.Models;

public class SkillUser
{
	public string UserId { get; set; }
	public User User { get; set; }

	public Guid SkillId { get; set; }
	public Skill Skill { get; set; }
}