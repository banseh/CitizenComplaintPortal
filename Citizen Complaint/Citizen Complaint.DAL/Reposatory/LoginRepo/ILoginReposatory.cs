using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL.Models;

namespace Citizen_Complaint.DAL.Reposatory.LoginRepo
{
    public interface ILoginReposatory
    {
        Task<GeneralResult<LoginResponse>> LoginAsync(string email, string password);
    }
}
