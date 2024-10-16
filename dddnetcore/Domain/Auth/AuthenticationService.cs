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
    }
}
