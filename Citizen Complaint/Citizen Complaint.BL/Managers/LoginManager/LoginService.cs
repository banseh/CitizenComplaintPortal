using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.LoginDto;
using Citizen_Complaint.BL.Managers.JWTManager;
using Citizen_Complaint.DAL.Unit_OfWork;

namespace Citizen_Complaint.BL.Managers.LoginManager
{
    public class LoginService : ILoginService
    {
        private readonly IJwtService _jwtService;
        private readonly IUnitofWok _unitOfWork;
        public LoginService(IJwtService jwtService, IUnitofWok unitOfWork)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResult<LoginShowDto>> LoginAsync(string email, string password)
        {
            var result = await _unitOfWork.logrep.LoginAsync(email, password);
            if (!result.Status)
                return new GeneralResult<LoginShowDto>
                {
                    Status = false,
                    Data = null,
                    Errors = result.Errors
                };
            else
            {
                var token = _jwtService.GenerateToken(email, result.Data.Id);
                return new GeneralResult<LoginShowDto>
                {
                    Status = true,
                    Data = new LoginShowDto
                    {
                        Token = token,
                        Id = result.Data.Id,
                        FullName = result.Data.FullName
                    },
                    Errors = null
                };
            }
        }
    }
}
