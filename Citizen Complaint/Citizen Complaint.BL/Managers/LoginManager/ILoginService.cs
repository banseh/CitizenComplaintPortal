using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.LoginDto;

namespace Citizen_Complaint.BL.Managers.LoginManager
{
    public interface ILoginService
    {
        Task<GeneralResult<LoginShowDto>> LoginAsync(string email, string password);
    }

}
