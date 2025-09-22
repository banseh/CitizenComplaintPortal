using Citizen_Complaint.BL.Common;

namespace Citizen_Complaint.BL.Managers.CompalinManager
{
    public interface IComplaintService
    {
        Task<GeneralResult<string>> SubmitComplaintAsync(ComplaintDto complaint, string userid);
        Task<GeneralResult<ComplaintDto>> GetCaseByIdAsync(string id);
        Task<GeneralResult<ComplaintDto>> GetCaseByTicketNumberAsync(string ticketNumber);
        Task<GeneralResult<ComplaintDto>> GetCaseByTicketNumberAndEmailAsyncc(string ticketNumber, string email);
        Task<GeneralResult<List<ComplaintDto>>> GetCasesByCustomerAsync(string customerId);
        Task<GeneralResult<string>> DeleteComplaintAsync(string complaintId);
        Task<GeneralResult<string>> SubmitComplaintANYNOUMS(ComplaintDto complaint, string email);
        Task<bool> VerifyTokenAndCheckContactAsync(string token);
    }
}
