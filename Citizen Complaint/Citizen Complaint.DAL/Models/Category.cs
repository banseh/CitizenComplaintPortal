using Newtonsoft.Json;


namespace Citizen_Complaint.DAL
{
    public class Category
    {
        [JsonProperty("categoryid")]
        public Guid CategoryId { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("cp_cat_description")]
        public string Description { get; set; } = string.Empty;
    }
}
