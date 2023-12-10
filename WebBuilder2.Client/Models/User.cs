using System.IdentityModel.Tokens.Jwt;

namespace WebBuilder2.Client.Models;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public static User? FromGoogleJwt(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            return new()
            {
                Username = jwtSecurityToken.Claims.First(c => c.Type == "name").Value,
                Password = ""
            };
        }

        return null;
    }
}
