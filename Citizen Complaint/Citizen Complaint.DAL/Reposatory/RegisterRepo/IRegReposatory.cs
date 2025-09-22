using Citizen_Complaint.BL.Common;

namespace Citizen_Complaint.DAL.Reposatory.RegisterRepo
{
    public interface IRegReposatory
    {
        Task<GeneralResult<string>> CreateContactAsync(Register register);
        Task<GeneralResult<Register>> GetContactByEmailAsync(string mail);
        Task<GeneralResult<string>> UpdateAnonymousContactAsync(string contactId, Register register);
        Task<GeneralResult<string>> UpdateprofileAsync(Register register);
    }
}
