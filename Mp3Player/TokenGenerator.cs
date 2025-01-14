namespace Mp3Player;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenGenerator
{
    private const string SecretKey = "snko%4i5s_pm%otrpt8%9ga4hh$((jad+c%1dkcoo&7@aoj9nw"; 
    private const string Issuer = "Dar1en9"; 
    private const string Audience = "Mp3PlayerUser"; 

    public string GenerateToken(string username, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username), 
            new(ClaimTypes.Role, role), 
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer, 
            audience: Audience, 
            claims: claims, 
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
