using Newtonsoft.Json;

namespace Citizen_Complaint.DAL
{
    public class Complaint
    {
        [JsonProperty("incidentid")]
        public Guid ComplaintId { get; set; }
        [JsonProperty("customerid_contact@odata.bind")]
        public string CustomerBinding { get; set; }
        [JsonProperty("title")]
        public required string Name { get; set; }
        [JsonProperty("emailaddress")]
        public required string Email { get; set; }
        [JsonProperty("cp_nationalid")]
        public required string Nationalid { get; set; }
        [JsonProperty("cp_description")]
        public required string Description { get; set; }
        [JsonProperty("cp_location")]
        public required string Location { get; set; }
        [JsonProperty("cp_Category@odata.bind")]
        public string CategoryBinding { get; set; }
        [JsonProperty("ticketnumber")]
        public string TicketNumber { get; set; }
        [JsonProperty("statuscode")]
        public int? StatusCode { get; set; }
    }

}
