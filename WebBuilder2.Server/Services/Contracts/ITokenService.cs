using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Services.Contracts;

public interface ITokenService
{
    public string GenerateToken(ApplicationUser user);
}
