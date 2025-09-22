using Newtonsoft.Json;

namespace Citizen_Complaint.DAL
{
    public class Register
    {
        [JsonIgnore]
        public  Guid ?Contactid { get; set; }
        [JsonProperty("firstname")]
        public required string Firstname { get; set; }
        [JsonProperty("lastname")]
        public required string Lastname { get; set; }
        [JsonProperty("emailaddress1")]
        public required string Email { get; set; }
        [JsonProperty("adx_identity_passwordhash")]
        public required string Password { get; set; }
        [JsonProperty("gendercode")]
        public int Gender { get; set; }
        [JsonProperty("cp_isayumus")]
        public bool IsAnynoums { get; set; }
    }
}
