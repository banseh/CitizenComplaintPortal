using System.Net.Http.Headers;
using System.Text;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL.Azure_Context;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Citizen_Complaint.DAL.Reposatory.CompalintRepo
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly Azure _azure;
        private readonly IConfiguration _config;
        public ComplaintRepository(Azure azure, IConfiguration config)
        {
            _azure = azure;
            _config = config;
        }
        public async Task<GeneralResult<string>> CreateComplaintAsync(Complaint complaint)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();
            var jsonComplaint = JsonConvert.SerializeObject(complaint);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            client.DefaultRequestHeaders.Add("Prefer", "return=representation");

            string apiUrl = _azure._config["Azure:DynamicsUrl"] + "/api/data/v9.1/incidents";

            var content = new StringContent(jsonComplaint, Encoding.UTF8, "application/json") as HttpContent;

            Console.WriteLine("jsonComplaint=> " + jsonComplaint);
            Console.WriteLine("content=> " + content);

            var response = await client.PostAsync(apiUrl, content);
            Console.WriteLine("response=> " + response);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<string>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Failed to create complaint."+error,
                            Code = response.StatusCode.ToString()
                        }
                    }
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);
            string ticketNumber = json["ticketnumber"]?.ToString();
            return new GeneralResult<string>
            {
                Status = true,
                Data = ticketNumber
            };
        }


        public async Task<bool> IsContactExistsAsync(string contactIdOrEmail)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            string filter;
            if (Guid.TryParse(contactIdOrEmail, out Guid contactId))
            {
                filter = $"contactid eq {contactId}";
            }
            else
            {
                filter = $"emailaddress1 eq '{contactIdOrEmail}'";
            }

            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/contacts?$filter={filter}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);
            return result.value.Count > 0;
        }


        public async Task<GeneralResult<Complaint>> GetCaseByIdAsyncc(string id)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/incidents({id})" +
             "?$select=title,cp_description,ticketnumber,emailaddress,cp_location,cp_nationalid,statuscode" +
             "&$expand=cp_Category($select=title)";
            Console.WriteLine("url=> " + url);
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Failed to retrieve case." + error,
                            Code = response.StatusCode.ToString()
                        }
                    }
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);
            Console.WriteLine("data=> " + data);
            if (data == null || data.title == null)
            {
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "No case found with the provided ID.",
                            Code = "404"
                        }
                    }
                };
            }

            return new GeneralResult<Complaint>
            {
                Status = true,
                Data = new Complaint
                {
                    Name = data.title,
                    Description = data.cp_description,
                    Email = data.emailaddress,
                    Nationalid = data.cp_nationalid,
                    Location = data.cp_location,
                    TicketNumber = data.ticketnumber,
                    ComplaintId = data.incidentid,
                    CategoryBinding = (data.cp_Category is JObject categoryObj) ? data.cp_Category.title?.ToString() : null,
                    StatusCode = data.statuscode
                }
            };

        }


        public async Task<GeneralResult<Complaint>> GetCaseByTicketNumberAsyncc(string ticketNumber)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            string encodedTicketNumber = Uri.EscapeDataString(ticketNumber);

            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/incidents" +
                       $"?$filter=ticketnumber eq '{ticketNumber}'" +
                       "&$select=title,cp_description,ticketnumber,emailaddress,cp_location,cp_nationalid,statuscode" +
                       "&$expand=cp_Category($select=title)";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Failed to retrieve case by ticket number." + error,
                            Code = response.StatusCode.ToString()
                        }
                    }
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);
            if (result.value.Count == 0)
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "No case found with the provided ticket number.",
                            Code = "404"
                        }
                    }
                };

            dynamic data = result.value[0];
            return new GeneralResult<Complaint>
            {
                Status = true,
                Data = new Complaint
                {
                    Name = data.title,
                    Description = data.cp_description,
                    Email = data.emailaddress,
                    Nationalid = data.cp_nationalid,
                    Location = data.cp_location,
                    TicketNumber = data.ticketnumber,
                    ComplaintId = data.incidentid,
                    CategoryBinding = (data.cp_Category is JObject categoryObj) ? data.cp_Category.title?.ToString() : null,
                    StatusCode = data.statuscode
                }
            };
        }
        public async Task<GeneralResult<Complaint>> GetCaseByTicketNumberAndEmailAsyncc(string ticketNumber, string email)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/incidents" +
                          $"?$filter=ticketnumber eq '{ticketNumber}' and emailaddress eq '{email}'" +
                          "&$select=title,cp_description,ticketnumber,emailaddress,cp_location,cp_nationalid,statuscode" +
                          "&$expand=cp_Category($select=title)";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {error}");
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Failed to retrieve case by ticket number and email." + error,
                            Code = response.StatusCode.ToString()
                        }
                    }
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "No data returned from the API.",
                            Code = "404"
                        }
                    }
                };

            dynamic result = JsonConvert.DeserializeObject(json);
            if (result?.value == null || result.value.Count == 0)
                return new GeneralResult<Complaint>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "No case found with the provided ticket number and email.",
                            Code = "404"
                        }
                    }
                };

            dynamic data = result.value[0];
            return new GeneralResult<Complaint>
            {
                Status = true,
                Data = new Complaint
                {
                    Name = data.title,
                    Description = data.cp_description,
                    Email = data.emailaddress,
                    Nationalid = data.cp_nationalid,
                    Location = data.cp_location,
                    TicketNumber = data.ticketnumber,
                    ComplaintId = data.incidentid,
                    CategoryBinding = (data.cp_Category is JObject categoryObj) ? data.cp_Category.title?.ToString() : null,
                    StatusCode = data.statuscode

                }
            };
        }

        public async Task<GeneralResult<List<Complaint>>> GetCasesByCustomerAsyncc(string customerId)
        {
            string accessToken = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            if (!Guid.TryParse(customerId, out Guid parsedId))
                return new GeneralResult<List<Complaint>>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Invalid customer ID format.",
                            Code = "400"
                        }
                    }
                };

            string url = $"{_config["Azure:DynamicsUrl"]}/api/data/v9.1/incidents" +
              $"?$filter=_customerid_value eq '{parsedId}'" +
              "&$select=title,cp_description,ticketnumber,emailaddress,cp_location,cp_nationalid,incidentid,statuscode,_customerid_value" +
              "&$expand=cp_Category($select=title)";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<List<Complaint>>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Failed to retrieve cases by customer." + error,
                            Code = response.StatusCode.ToString()
                        }
                    }
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);
            var complaints = new List<Complaint>();
            foreach (var item in result.value)
            {
                complaints.Add(new Complaint
                {
                    Name = item.title,
                    Description = item.cp_description,
                    Email = item.emailaddress,
                    Nationalid = item.cp_nationalid,
                    Location = item.cp_location,
                    TicketNumber = item.ticketnumber,
                    ComplaintId = item.incidentid,
                    CategoryBinding = (item.cp_Category is JObject categoryObj) ? item.cp_Category.title?.ToString() : null,
                    StatusCode = item.statuscode,
                    CustomerBinding = item.ToString().Contains("_customerid_value") ? item._customerid_value.ToString() : null
                });
            }
            return new GeneralResult<List<Complaint>>
            {
                Status = true,
                Data = complaints
            };
        }

        public async Task<GeneralResult<string>> DeleteComplaintAsync(string complaintId)
        {
            var token = await _azure.GetAccessTokenFromAzureAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dynamicsUrl = _config["Azure:DynamicsUrl"];
            var requestUrl = $"{dynamicsUrl}/api/data/v9.1/incidents({complaintId})";

            var response = await client.DeleteAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return new GeneralResult<string>
                {
                    Status = true,
                    Data = "Complaint deleted successfully."
                };
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResult<string>
                {
                    Status = false,
                    Errors = new[]
                    {
                        new ResultError
                        {
                            Message = "Failed to delete complaint. " + error,
                            Code = response.StatusCode.ToString()
                        }
                    }
                };
            }
        }

    }
}
