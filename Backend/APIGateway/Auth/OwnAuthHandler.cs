using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Newtonsoft.Json.Linq;





namespace OpenVidStreamer.APIGateway.Auth;
 
    
    public class OwnAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {


        private readonly string _jwtSecret; 
        
        IConfiguration _configuration;
        
        public OwnAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                              ILoggerFactory logger, 
                              UrlEncoder encoder, 
                              ISystemClock clock,
                              IConfiguration configuration) 
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
            _jwtSecret = Environment.GetEnvironmentVariable("JwtSecret") ?? configuration["CustomJWT:Secret"];
        }



     
   
        
      
  
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            
            var token = Context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token)) //signalR cant send headers so the token is sent in the query string
            {
                string? signalRacessToken = Context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(signalRacessToken))
                {
                    token = signalRacessToken;
                }
            }


            
            
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing or malformed 'Authorization' header."));
            }
            try
            {
                
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["CustomJWT:Issuer"], 
                    ValidAudience = _configuration["CustomJWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)) 
                };

                var handler = new JwtSecurityTokenHandler();
                var claimsPrincipal  = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                Context.User = claimsPrincipal;
                var a = Context.User;
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(Context.User, Scheme.Name)));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex.Message));
            }
        }
    }

