using Microsoft.IdentityModel.Tokens;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPATIUM_backend.Security;

public class TokenService : ITokenService
{
	private readonly IConfiguration _cfg;

	public TokenService(IConfiguration cfg)
	{
		_cfg = cfg;
	}

	public string CreateToken(User user)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.Name, user.UserName!),
			new(ClaimTypes.NameIdentifier, user.Id),
			new(ClaimTypes.Email, user.Email!),
		};
		
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
		var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = signingCredentials,
			Issuer = _cfg["Jwt:Issuer"],
			Audience = _cfg["Jwt:Issuer"]
		};

		var tokenHandler = new JwtSecurityTokenHandler();

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}