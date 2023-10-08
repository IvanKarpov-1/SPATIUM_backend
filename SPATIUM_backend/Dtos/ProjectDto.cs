namespace SPATIUM_backend.Dtos;

public class ProjectDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Creator { get; set; }
	public List<string> RequiredSkills { get; set; }
	public DateTime CreationDate { get; set; }
	public string ImageUrl { get; set; }
}