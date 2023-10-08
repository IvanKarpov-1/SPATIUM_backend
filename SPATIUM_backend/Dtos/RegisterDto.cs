using System.ComponentModel.DataAnnotations;

namespace SPATIUM_backend.Dtos;

public class RegisterDto
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Пароль повинен бути складним та складатися з, хоча б, однієї латинської великої букви, однієї малої букви та однієї цифр")]
	public string Password { get; set; }
	
	[Required]
	public string Username { get; set; }
}