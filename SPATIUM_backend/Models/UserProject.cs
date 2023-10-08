namespace SPATIUM_backend.Models;

public class UserProject
{
	public string UserId { get; set; }
	public User User { get; set; }

	public Guid ProjectId { get; set; }
	public Project Project { get; set; }

	public bool IsAccepted { get; set; }
	public bool IsHost { get; set; }
}