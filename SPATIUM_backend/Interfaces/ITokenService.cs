using SPATIUM_backend.Models;

namespace SPATIUM_backend.Interfaces;

public interface ITokenService
{
	string CreateToken(User user);
}