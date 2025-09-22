using System.Net.Http.Headers;
using System.Text;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL.Azure_Context;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Citizen_Complaint.DAL.Reposatory.RegisterRepo
{
    public class RegisterReposatory : IRegReposatory
    {

        private readonly Azure _azure;
        private readonly IConfiguration _config;

        public RegisterReposatory(Azure azure, IConfiguration config)
        {
            _azure = azure;
            _config = config;
        }
        public async Task<GeneralResult<Register?>> GetContactByEmailAsync(string mail)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var encodedEmail = Uri.EscapeDataString(mail);
            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/contacts?$filter=emailaddress1 eq '{mail}'&$select=firstname ,lastname,emailaddress1,cp_isayumus,adx_identity_passwordhash,gendercode ";
            Console.WriteLine("Requesting URL: " + url);

            var response = await client.GetAsync(url);
            Console.WriteLine("Responssssssssssse=> " + response);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Failed to fetch contact.");
                return new GeneralResult<Register?>
                {
                    Status = false,
                    Errors = new ResultError[]
                    {
                        new ResultError
                        {
                            Code = response.StatusCode.ToString(),
                            Message = "Failed to fetch contact. " + await response.Content.ReadAsStringAsync(),
                        }
                    }
                };
            }

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);
            if (result.value.Count == 0)
                return new GeneralResult<Register?>
                {
                    Status = false,
                    Errors = new ResultError[]
                    {
                        new ResultError
                        {
                            Code = "NotFound",
                            Message = "Contact not found."
                        }
                    }
                };

            var contact = result.value[0];
            Console.WriteLine("contentttttttt=> " + contact);
            var register = new Register
            {

                Firstname = contact.firstname,
                Lastname = contact.lastname,
                Email = contact.emailaddress1,
                IsAnynoums = contact.cp_isayumus,
                Password = contact.adx_identity_passwordhash,
                Gender = contact.gendercode,
                Contactid = contact.contactid,
            };
            return new GeneralResult<Register?>
            {
                Status = true,
                Data = register
            };
        }
        public async Task<GeneralResult<string>> CreateContactAsync(Register register)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();
            var jsonBody = JsonConvert.SerializeObject(register);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            string url = _config["Azure:DynamicsUrl"] + "/api/data/v9.1/contacts";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<string>
                {
                    Status = false,
                    Errors = new ResultError[]
                    {
                        new ResultError
                        {
                            Code = response.StatusCode.ToString(),
                            Message = "Failed to create contact. " + error,
                        }
                    }
                };
            }
            if (response.Headers.TryGetValues("OData-EntityId", out var values))
            {
                var entityId = values.FirstOrDefault();
                if (!string.IsNullOrEmpty(entityId))
                {
                    var contactId = entityId.Split('(')[1].TrimEnd(')');
                    return new GeneralResult<string>
                    {
                        Status = true,
                        Data = contactId
                    };
                }
            }
            return new GeneralResult<string>
            {
                Status = true,
                Data = "create contact Sucessfuly"
            };

        }
        public async Task<GeneralResult<string>> UpdateprofileAsync(Register register)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            
            Console.WriteLine("register.Email=> " + register.Email);
            Console.WriteLine("register.Contactid=> " + register.Contactid);
            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/contacts({register.Contactid})";
            var updatedFields = new
            {
                firstname = register.Firstname,
                lastname = register.Lastname,
                gendercode = register.Gender,

            };

            var jsonBody = JsonConvert.SerializeObject(updatedFields);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new GeneralResult<string>
                {
                    Status = true,
                    Data = "Profile updated successfully."
                };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResult<string>
            {
                Status = false,
                Errors = new ResultError[]
                {
                    new ResultError
                    {
                        Code = "",
                        Message = "Failed to update contact."
                    }
                }
            };
        }
        public async Task<GeneralResult<string>> UpdateAnonymousContactAsync(string contactId, Register register)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/contacts({contactId})";
            var updateObject = new
            {
                firstname = register.Firstname,
                lastname = register.Lastname,
                emailaddress1 = register.Email,
                adx_identity_passwordhash = register.Password,
                gendercode = register.Gender,
                cp_isayumus = false
            };

            Console.WriteLine("register.Firstname=> " + register.Firstname);
            Console.WriteLine("updateObject.firstname=> " + updateObject.firstname);
            var jsonBody = JsonConvert.SerializeObject(register);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<string>
                {
                    Status = false,
                    Errors = new ResultError[]
                    {
                        new ResultError
                        {
                            Code =response.StatusCode.ToString(),
                            Message = error
                        }
                    }
                };
            }

            return new GeneralResult<string>
            {
                Status = true,
                Data = "You are registered successfully!"
            };
        }

    }
}
