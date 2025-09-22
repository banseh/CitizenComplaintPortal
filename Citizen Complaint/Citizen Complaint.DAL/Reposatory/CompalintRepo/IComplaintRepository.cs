using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citizen_Complaint.BL.Common;

namespace Citizen_Complaint.DAL.Reposatory.CompalintRepo
{
    public interface IComplaintRepository
    {
        Task<GeneralResult< string>> CreateComplaintAsync(Complaint complaint);
        Task<GeneralResult<Complaint>> GetCaseByIdAsyncc(string id);
        Task<GeneralResult<Complaint>> GetCaseByTicketNumberAsyncc(string ticketNumber);
        Task<GeneralResult<List<Complaint>>> GetCasesByCustomerAsyncc(string customerId);
        public Task<GeneralResult<Complaint>> GetCaseByTicketNumberAndEmailAsyncc(string ticketNumber, string email);
        Task<GeneralResult<string>> DeleteComplaintAsync(string complaintId);
        Task<bool> IsContactExistsAsync(string contactIdOrEmail);

    }
}
