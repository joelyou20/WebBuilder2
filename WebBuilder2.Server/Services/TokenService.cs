using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(ApplicationUser user)
    {
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ArgumentNullException("Username is null");
        }

        string? jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new KeyNotFoundException("JwtKey not found");
        }

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
