using Citizen_Complaint.BL;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.RegisterDto;
using Citizen_Complaint.BL.Managers.CompalinManager;
using Citizen_Complaint.BL.Managers.LoginManager;
using Citizen_Complaint.BL.Managers.RegisterManager;
using Citizen_Complaint.DAL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Citizen_Complaint.Controllers.RegisterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _regservice;
        public RegisterController(IComplaintService service, IRegisterService regservice, ILoginService loginservice, IExtractContactInfoFromToken tokenExtractor)
        {
            _regservice = regservice;
        }

        [HttpPost]
        public async Task<Results<Ok<GeneralResult<string>>, BadRequest<GeneralResult<string>>>> Redister([FromBody] Register register)
        {
            var result = await _regservice.CreateContactAsync(register);
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.BadRequest(result);
        }

        [HttpPost("RegisterAcc")]
        public async Task<Results<Ok<GeneralResult<string>>, BadRequest<GeneralResult<string>>>> RegisterAsync([FromBody] Register register)
        {
            var result = await _regservice.CreateOrUpdateContactAsync(register);

            if (result.Status)
            {
                return TypedResults.Ok(result);
            }
            else
            {
                return TypedResults.BadRequest(result);
            }

        }

        [HttpGet("cases/mailexist/{mail}")]
        public async Task<Results<Ok<GeneralResult<RegisterDTO_s>>, NotFound<GeneralResult<RegisterDTO_s>>>> ismailexistornot(string mail)
        {
            var exist = await _regservice.GetContactByEmailAsync(mail);
            if (exist.Status)
            {
                return TypedResults.Ok(exist);
            }
            return TypedResults.NotFound(exist);
        }
    }
}




