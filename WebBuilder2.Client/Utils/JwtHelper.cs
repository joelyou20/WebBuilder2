using System.IdentityModel.Tokens.Jwt;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Client.Utils
{
    public static class JwtHelper
    {
        public static UserModel? FromGoogleJwt(string token)
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
}
