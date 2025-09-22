using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.LoginDto;
using Citizen_Complaint.BL.Managers.LoginManager;
using Citizen_Complaint.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Citizen_Complaint.Controllers.LoginController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginservice;
        public LoginController( ILoginService loginservice)
        {
            _loginservice = loginservice;
        }

        [HttpPost]
        public async Task<Results<Ok<GeneralResult<LoginShowDto>>, BadRequest<GeneralResult<LoginShowDto>>>> Login([FromBody] Login log)
        {
            var result = await _loginservice.LoginAsync(log.Email, log.Password);
            if (result.Status)
            {
                return TypedResults.Ok(result);
            }
            else
            {
                return TypedResults.BadRequest(result);
            }
        }
    }
}



