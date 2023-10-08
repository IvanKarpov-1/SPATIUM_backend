namespace SPATIUM_backend.Dtos;

public class ProfileDto
{
	public Guid Id { get; set; }
	public DateTime CreationDate { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public List<string> Skills { get; set; }	
	public string ImageUrl { get; set; }
}