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
        
        IConfiguration _configuration;
        
        public OwnAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                              ILoggerFactory logger, 
                              UrlEncoder encoder, 
                              ISystemClock clock,
                              IConfiguration configuration) 
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
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



            string ExtractTokenFromJson(string jsonString)
            {
                var jObject = JObject.Parse(jsonString);
                return jObject["token"]?.ToString();
            }
            if (Context.Request.Path.ToString().Contains("/beacon"))  //navigator.sendBeacon cant send headers [Used for saving& unlocking document on window.onbeforeunload]
            {
                //backup the original request body
                var originalRequestBody = Context.Request.Body;
                try   //ONCE THE BODY IS READ IT CANT BE READ AGAIN , and since navigator.sendBeacon cant send headers cant send the token in the header , i encoded the token in the body 
                {
                    Context.Request.EnableBuffering(); // Enable buffering so the body can be read multiple times
                
                //parse the token from the body
                using var reader = new StreamReader(Context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var bodyString =  reader.ReadToEndAsync().Result;
                 token =  ExtractTokenFromJson(bodyString);
                 
                 
                 
                 // Reset the request body stream position to 0
                 Context.Request.Body.Position = 0;

                 // Replace the request body with a new MemoryStream containing the original content
                 var requestData = Encoding.UTF8.GetBytes(bodyString);
                 var newRequestBody = new MemoryStream(requestData);
                 Context.Request.Body = newRequestBody; 
                }
                catch (Exception e)
                {
                    Context.Request.Body = originalRequestBody;
                    return Task.FromResult(AuthenticateResult.Fail(e.Message));
                }
            }
            
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing or malformed 'Authorization' header."));
            }

            
            //allow authentication with api secret if enabled
            if (Convert.ToBoolean(_configuration["EnableDirectAcessToApiWithApiSecret"]))
            {
                if (_configuration["ApiSecret"] is not null && _configuration["ApiSecret"] != "")
                {
                    if(token == _configuration["ApiSecret"])
                    {
                        var identity = new ClaimsIdentity("DirectApiSecret");  // Create an identity, if needed
                        var principal = new ClaimsPrincipal(identity);
                        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
                    }
                }
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["CustomJWT:Secret"])) 
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

