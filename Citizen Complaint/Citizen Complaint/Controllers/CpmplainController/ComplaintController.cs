using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Citizen_Complaint.BL;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Managers.CompalinManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Citizen_Complaint.Controllers.CpmplainController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _service;
        public ComplaintController(IComplaintService service)
        {
            _service = service;
        }

        [HttpPost("SubmitComplaint")]
        [Authorize]
        public async Task<Results<
        Ok<GeneralResult<string>>,
        UnauthorizedHttpResult,
        BadRequest<GeneralResult<string>>>> SubmitComplaint([FromBody] ComplaintDto complaint)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var isValid = await _service.VerifyTokenAndCheckContactAsync(token);
            if (!isValid)
                return TypedResults.Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var contactId = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier
            )?.Value;
            Console.WriteLine("contactId=> ", contactId);

            var result = await _service.SubmitComplaintAsync(complaint, $"/contacts({contactId})");
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.BadRequest(result);
        }

        [HttpGet("case/{id}")]
        public async Task<Results<
        Ok<GeneralResult<ComplaintDto>>,
        NotFound<GeneralResult<ComplaintDto>>>> GetCaseById(string id)
        {
            var caseDetails = await _service.GetCaseByIdAsync(id);
            if (caseDetails == null)
                return TypedResults.Ok(caseDetails);
            return TypedResults.NotFound(caseDetails);
        }

        [HttpGet("case/ticket/{ticketNumber}")]
        public async Task<Results<
        Ok<GeneralResult<ComplaintDto>>,
        NotFound<GeneralResult<ComplaintDto>>>> GetCaseByTicketNumber(string ticketNumber)
        {
            var result = await _service.GetCaseByTicketNumberAsync(ticketNumber);
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.NotFound(result);
        }

        [HttpGet("case/ticketNumber/{ticketNumber}/email/{email}")]
        public async Task<Results<
        Ok<GeneralResult<ComplaintDto>>,
        NotFound<GeneralResult<ComplaintDto>>>> GetCaseByTicketNumberAndEmail(string ticketNumber, string email)
        {
            var result = await _service.GetCaseByTicketNumberAndEmailAsyncc(ticketNumber, email);
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.NotFound(result);
        }


        [HttpGet("cases/customer/{customerId}")]
        public async Task<Results<
        Ok<GeneralResult<List<ComplaintDto>>>,
        NotFound<GeneralResult<List<ComplaintDto>>>>> GetCasesByCustomer(string customerId)
        {
            var result = await _service.GetCasesByCustomerAsync(customerId);
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.NotFound(result);
        }

        [HttpPost("submit-anonymous")]
        public async Task<Results<
        Ok<GeneralResult<string>>,
        BadRequest<GeneralResult<string>>>> SubmitAnonymousComplaint([FromBody] ComplaintDto complaintDto)
        {
            var result = await _service.SubmitComplaintANYNOUMS(complaintDto, complaintDto.Email);
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<Results<
        Ok<GeneralResult<string>>,
        BadRequest<GeneralResult<string>>>> DeleteComplaint(string id)
        {
            var result = await _service.DeleteComplaintAsync(id);
            if (!result.Status)
            {
                return TypedResults.BadRequest(result);
            }
            return TypedResults.Ok(result);
        }
    }
}
