namespace Citizen_Complaint.BL.Dtos.RegisterDto
{
    public class RegisterDTO_s
    {
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int Gender { get; set; }
        public bool IsAnynoums { get; set; }
    }
}
