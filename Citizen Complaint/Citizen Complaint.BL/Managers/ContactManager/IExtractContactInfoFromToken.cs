using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.RegisterDto;

namespace Citizen_Complaint.BL
{
    public interface IExtractContactInfoFromToken
    {
        public Task<GeneralResult<RegisterDto>> ExtractContactInfoFromToken(string token);
    }
}
