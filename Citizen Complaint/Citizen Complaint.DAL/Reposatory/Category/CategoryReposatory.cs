using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL;
using Citizen_Complaint.DAL.Azure_Context;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Citizen_Complaint.DAL;

public class CategoryReposatory : ICategoryReposatory
{
    private readonly Azure _azure;
    private readonly IConfiguration _config;
    public CategoryReposatory(Azure azure, IConfiguration config)
    {
        _azure = azure;
        _config = config;
    }
    public async Task<GeneralResult<List<Category>>> GetAllCategoriesAsync()
    {
        string accessToken = await _azure.GetAccessTokenFromAzureAsync();

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        var url = _config["Azure:DynamicsUrl"] + "/api/data/v9.1/categories?$select=categoryid,title,cp_cat_description";
        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResult<List<Category>>
            {
                Status = false,
                Errors = new[] { new ResultError { Message = "Failed to retrieve categories", Code = response.StatusCode.ToString() } }
            };
        }

        var json = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(json);
        List<Category> categories = new List<Category>();
        foreach (var item in result.value)
        {
            categories.Add(new Category
            {
                Name = item.title,
                CategoryId = item.categoryid,
                Description = item.cp_cat_description
            });
        }
        return new GeneralResult<List<Category>>
        {
            Status = true,
            Data = categories
        };
    }

}
