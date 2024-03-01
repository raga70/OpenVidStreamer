using Microsoft.CSharp.RuntimeBinder;

namespace Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public class AccIdExtractorFromHttpContext
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext">ofType(HttpContext) </param>
    /// <returns></returns>
    public static string GetAccId(dynamic httpContext) //the class library doesnt have the HttpContext from ASP.NET class so we use dynamic
    {
        try
        {
            // Attempt to dynamically access the Claims
            var userClaims = httpContext?.User?.Claims as IEnumerable<Claim>;
            if (userClaims != null)
            {
                var claim = userClaims
                    .FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn");

                return claim?.Value ?? "unknownAuthenticatedUser";
            }
        }
        catch (RuntimeBinderException)
        {
            // Handle cases where dynamic invocation fails due to missing members
            Console.WriteLine("Error accessing properties on dynamic type.");
        }
        catch (Exception ex)
        {
            // General exception handling
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return "unknownAuthenticatedUser";
    }
}
