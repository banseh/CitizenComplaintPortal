using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Citizen_Complaint.DAL.Azure_Context
{
    public class Azure
    {
        public readonly IConfiguration _config;
        public Azure(IConfiguration config)
        {
            _config = config;
        } 
        public async Task<string> GetAccessTokenFromAzureAsync()
        {
            var tenantId = _config["Azure:TenantId"];
            var clientId = _config["Azure:ClientId"];
            var clientSecret = _config["Azure:ClientSecret"];
            var resource = _config["Azure:DynamicsUrl"];
            var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            var values = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "client_credentials" },
                { "scope", resource + "/.default" }
            };

            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(180)
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to acquire token. Status: {response.StatusCode}, Details: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            return result.access_token;
        }

    }
}
