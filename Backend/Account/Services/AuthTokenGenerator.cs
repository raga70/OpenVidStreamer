using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Account.Services;

public static class AuthTokenGenerator
{
    public static string GenerateOwnAuthToken(string accId, IConfiguration configuration,bool hasActiveSubscription = false)
    {
         string  _jwtSecret = Environment.GetEnvironmentVariable("JwtSecret") ?? configuration["CustomJWT:Secret"];
         string _jwtExpiration = Environment.GetEnvironmentVariable("JwtExpiration") ??
                                 configuration["CustomJWT:ExpirationInHours"];
        
        
        // Define token claims
        var claims = new List<Claim>
        {
            new Claim("upn", accId),
            new Claim(JwtRegisteredClaimNames.Sub, accId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        
        // Add the "activeSubscription" role claim if the user has an active subscription
        if (hasActiveSubscription)
        {
            claims.Add(new Claim("isSubscribed", "activeSubscription"));
        }
        
        
        // Generate token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["CustomJWT:Issuer"],
            audience: configuration["CustomJWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_jwtExpiration)),  // Token expiry time
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static string GenerateSignalRAuthToken(string accId, IConfiguration configuration)
    {
        // Define token claims
        var claims = new[]
        {
            new Claim("upn", accId),
            new Claim(JwtRegisteredClaimNames.Sub, accId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Generate token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["CustomJWT:SignalRsecret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["CustomJWT:Issuer"],
            audience: configuration["CustomJWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),  // Token expiry time
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
  
    
}