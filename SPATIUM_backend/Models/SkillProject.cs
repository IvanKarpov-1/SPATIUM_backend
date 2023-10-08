namespace SPATIUM_backend.Models;

public class SkillProject
{
	public Guid ProjectId { get; set; }
	public Project Project { get; set; }

	public Guid SkillId { get; set; }
	public Skill Skill { get; set; }
}