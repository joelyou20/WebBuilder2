using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebBuilder2.Server.Utils;

public class TokenAuthorizationFilter(IConfiguration configuration) : IAsyncAuthorizationFilter
{
    private readonly IConfiguration _configuration = configuration;

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return Task.CompletedTask;

        if (string.IsNullOrEmpty(token) || !ValidateToken(token))
        {
            // Return 401 Unauthorized if token is missing or invalid
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    private bool ValidateToken(string token)
    {
        var key = _configuration["Jwt:Key"];
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(keyBytes);

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Issuer"],
                IssuerSigningKey = securityKey
            }, out SecurityToken validatedToken);

            return true; // Token is valid
        }
        catch
        {
            return false; // Token is invalid
        }
    }
}