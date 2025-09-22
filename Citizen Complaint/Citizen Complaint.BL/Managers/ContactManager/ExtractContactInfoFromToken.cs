using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.RegisterDto;
using Citizen_Complaint.BL.Managers.RegisterManager;

namespace Citizen_Complaint.BL
{
    public class ExtractContactInfoFromToken : IExtractContactInfoFromToken
    {
        private readonly IRegisterService regmail;
        public ExtractContactInfoFromToken(IRegisterService _regmail)
        {
            regmail = _regmail;
        }
        async Task<GeneralResult<RegisterDto>> IExtractContactInfoFromToken.ExtractContactInfoFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return new GeneralResult<RegisterDto>
                {
                    Status = false,
                    Errors = new[] { new ResultError { Message = "Invalid JWT token", Code = "400" } }
                };

            var jwtToken = handler.ReadJwtToken(token);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "preferred_username")?.Value;
            if (string.IsNullOrEmpty(email))
                return new GeneralResult<RegisterDto>
                {
                    Status = false,
                    Errors = new[] { new ResultError { Message = "Email not found in token", Code = "400" } }
                };

            var result = await regmail.GetContactByEmailAsync(email);
            if (!result.Status)
                return new GeneralResult<RegisterDto>
                {
                    Status = false,
                    Errors = result.Errors
                };

            return new GeneralResult<RegisterDto>
            {
                Status = true,
                Data = new RegisterDto
                {
                    Firstname = result.Data.Firstname,
                    Lastname = result.Data.Lastname,
                    Email = result.Data.Email,
                    Gender = result.Data.Gender,
                    IsAnynoums = result.Data.IsAnynoums
                }
            };
        }
    }
}
