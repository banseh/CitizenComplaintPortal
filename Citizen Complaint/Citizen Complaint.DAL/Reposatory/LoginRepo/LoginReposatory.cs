using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL.Azure_Context;
using Citizen_Complaint.DAL.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Citizen_Complaint.DAL.Reposatory.LoginRepo
{
    public class LoginReposatory : ILoginReposatory
    {

        private readonly Azure _azure;
        private readonly IConfiguration _config;

        public LoginReposatory(Azure azure, IConfiguration config)
        {
            _azure = azure;
            _config = config;
        }
        public async Task<GeneralResult<LoginResponse>> LoginAsync(string email, string password)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            string encodedPassword = Uri.EscapeDataString(password);
            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/contacts?$filter=emailaddress1 eq '{email}' and adx_identity_passwordhash eq '{encodedPassword}'";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<LoginResponse>
                {
                    Status = false,
                    Errors = new ResultError[]
                    {
                        new ResultError
                        {
                            Code = response.StatusCode.ToString(),
                            Message = error
                        }
                    },
                    Data = null
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);
            if (result.value.Count == 0)
                return new GeneralResult<LoginResponse>
                {
                    Status = false,
                    Errors = new ResultError[]
                    {
                        new ResultError
                        {
                            Code = "404",
                            Message = "Contact not found or invalid credentials."
                        }
                    },
                    Data = null
                };

            string contactId = result.value[0].contactid;
            string fullName = result.value[0].fullname;
            return new GeneralResult<LoginResponse>
            {
                Status = true,
                Data = new LoginResponse
                {
                    Id = contactId,
                    FullName = fullName
                }
            };
        }
    }
}
