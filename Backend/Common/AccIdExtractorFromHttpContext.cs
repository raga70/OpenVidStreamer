using System.IdentityModel.Tokens.Jwt;
using Microsoft.CSharp.RuntimeBinder;

namespace Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public class AccIdExtractorFromHttpContext
{



    
    
    
    public static string ExtractAccIdUpnFromJwtToken(string jwtToken)
    {
        //extracr Bearer from the token if it is present
        if (jwtToken.StartsWith("Bearer "))
        {
            jwtToken = jwtToken.Substring(7);
        }
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwtToken);
        var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;

        if (tokenS == null)
        {
            throw new ArgumentException("Invalid JWT token");
        }

        // Attempt to extract the UPN claim; the claim type might vary
        // Commonly, the UPN can be under "upn", "email", or another claim, depending on the token issuer
        var upnClaim = tokenS.Claims.FirstOrDefault(claim => claim.Type == "upn");

        if (upnClaim == null)
        {
            throw new InvalidOperationException("UPN claim not found in the JWT token");
        }

        return upnClaim.Value;
    }
    
    
}
