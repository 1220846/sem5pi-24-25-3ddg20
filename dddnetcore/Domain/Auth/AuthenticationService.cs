using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DDDSample1.Domain.Auth
{
    public class AuthenticationService: ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessToken(string domain,string audience, string clientId, string clientSecret)
        {
            var tokenEndpoint = $"https://{domain}/oauth/token";

            var requestBody = new StringContent(JsonConvert.SerializeObject(new{client_id = clientId,client_secret = clientSecret,audience,grant_type = "client_credentials"
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(tokenEndpoint, requestBody);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonConvert.DeserializeObject(content);
            return tokenResponse.access_token;
        }

        public string GetLoggedInUsername(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Token not found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Aqui, 'sub' é um claim comum em tokens para identificar o usuário
            var usernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub");
            
            if (usernameClaim == null)
                throw new InvalidOperationException("Username not found in token claims");

            return usernameClaim.Value["auth0|".Length..];
        }
    }
}
