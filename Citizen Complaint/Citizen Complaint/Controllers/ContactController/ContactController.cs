using Citizen_Complaint.BL;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.ContactDto;
using Citizen_Complaint.BL.Dtos.RegisterDto;
using Citizen_Complaint.BL.Managers.RegisterManager;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Citizen_Complaint.Controllers.ContactController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IExtractContactInfoFromToken tokenExtractor;
        private readonly IRegisterService _regservice;
        public ContactController(IExtractContactInfoFromToken tokenExtractor, IRegisterService regservice)
        {
            this.tokenExtractor = tokenExtractor;
            this._regservice = regservice;
        }

        [HttpGet("GetContactInfo")]
        public async Task<Results<
        Ok<GeneralResult<RegisterDto>>,
        UnauthorizedHttpResult,
        NotFound<GeneralResult<RegisterDto>>>> GetContactInfoFromToken()
        {
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    return TypedResults.Unauthorized();
                var token = authHeader.Replace("Bearer ", "");

                var result = await tokenExtractor.ExtractContactInfoFromToken(token);

                if (result.Status)
                    return TypedResults.Ok(result);
                return TypedResults.NotFound(new GeneralResult<RegisterDto>
                {
                    Status = false,
                    Errors = result.Errors
                });
            }
        }

        [HttpGet("contactidfromtoken")]
        public async Task<Results<
            Ok<GeneralResult<ContectFromToken>>,
            UnauthorizedHttpResult, NotFound<GeneralResult<ContectFromToken>>>> VerifyToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return TypedResults.Unauthorized();
            }
            var token = authHeader.Substring("Bearer ".Length);
            var result = await _regservice.getcontactidfromtoken(token);
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.NotFound(result);
        }
    }
}



