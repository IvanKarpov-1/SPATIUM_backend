namespace SPATIUM_backend.Models;

public class Project
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public User Creator { get; set; }
	public ICollection<SkillProject> RequiredSkills { get; set; } = new List<SkillProject>();
	public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
	public DateTime CreationDate { get; set; } = DateTime.Now;
	public string ImageUrl { get; set; }
}