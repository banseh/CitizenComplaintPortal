using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.ContactDto;
using Citizen_Complaint.BL.Dtos.RegisterDto;
using Citizen_Complaint.DAL;

namespace Citizen_Complaint.BL.Managers.RegisterManager
{
    public interface IRegisterService
    {
        Task<GeneralResult<string>> CreateContactAsync(Register register);
        Task<GeneralResult<string>> CreateOrUpdateContactAsync(Register register);
        Task<GeneralResult<RegisterDTO_s>> GetContactByEmailAsync(string mail);
        Task<GeneralResult<string>> UpdateContactAsync( string contactId , Register register);
        Task<GeneralResult<ContectFromToken>> getcontactidfromtoken(string token);
    }
}
