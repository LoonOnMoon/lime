using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Lime.Infrastructure.Identity.Configuration;
using Lime.Infrastructure.Identity.Models;

using Microsoft.IdentityModel.Tokens;

namespace Lime.Infrastructure.Identity.Common;

public class JwtTokenGenerator
{
    private readonly JwtOptions options;

    public JwtTokenGenerator(
        JwtOptions options)
    {
        this.options = options;
    }

    public string GenerateToken(LimeIdentityUser user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(this.options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var securityToken = new JwtSecurityToken(
            issuer: this.options.Issuer,
            audience: this.options.Audience,
            expires: DateTime.Now.Add(this.options.ExpiresIn),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}