using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http;

namespace PoC.Utilities
{
    public static class TokenUtils
    {
        public static TokenValidationParameters GetCognitoTokenValidationParams()
        {
            var region = "us-east-1";                       //Configuration.GetSection("AppConfig:Region").Value;
            var userPoolId = "us-east-1_7beT0GFME";         //Configuration.GetSection("AppConfig:UserPoolId").Value;
            var appClientId = "25hg8jh03j3367dap3n71ibbf8"; //Configuration.GetSection("AppConfig:AppClientId").Value;

            var cognitoIssuer = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
            var jwtKeySetUrl = $"{cognitoIssuer}/.well-known/jwks.json";
            var cognitoAudience = appClientId;

            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(jwtKeySetUrl).GetAwaiter().GetResult();   // get JsonWebKeySet from AWS
            var json = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys; // serialize the result

            return new TokenValidationParameters
            {
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    // cast the result to be the type expected by IssuerSigningKeyResolver
                    return (IEnumerable<SecurityKey>)keys;
                },
                ValidIssuer = cognitoIssuer,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false
                //ValidAudience = cognitoAudience
            };
        }
    }
}
