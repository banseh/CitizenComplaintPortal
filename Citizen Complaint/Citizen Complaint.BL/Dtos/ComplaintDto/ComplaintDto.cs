namespace Citizen_Complaint.BL
{
    public class ComplaintDto
    {
        public required string Name { get; set; }
        public  string? ComplaintId { get; set; }
        public required string Email { get; set; }
        public required string Nationalid { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public string CategoryBinding { get; set; }
        public int? StatusCode { get; set; }
        public string? TicketNumber { get; set; }
        public string? CustomerBinding { get; set; }
    }
}
