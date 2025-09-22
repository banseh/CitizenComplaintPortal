using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL;
using Citizen_Complaint.DAL.Unit_OfWork;
using FluentValidation;

namespace Citizen_Complaint.BL.Managers.CompalinManager
{
    public class ComplaintService : IComplaintService
    {
        private readonly IUnitofWok _unitOfWork;
        private readonly IValidator<ComplaintDto> validator;

        public ComplaintService(
            IUnitofWok unitOfWork,
            IValidator<ComplaintDto> validator
            )
        {
            _unitOfWork = unitOfWork;
            this.validator = validator;
        }

        public async Task<GeneralResult<string>> SubmitComplaintAsync(ComplaintDto complaint, string userid)
        {

            var validationResult = await validator.ValidateAsync(complaint);
            if (!validationResult.IsValid)
            {
                return new GeneralResult<string>
                {
                    Status = false,
                    Errors = validationResult.Errors.Select(e => new ResultError
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }

            var com = new Complaint { Description = complaint.Description, Name = complaint.Name, Nationalid = complaint.Nationalid, Location = complaint.Location, CategoryBinding = complaint.CategoryBinding, CustomerBinding = userid, Email = complaint.Email, ComplaintId = Guid.NewGuid() };
            return await _unitOfWork.Comprep.CreateComplaintAsync(com);
        }

        public async Task<bool> VerifyTokenAndCheckContactAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var contactId = jwtToken.Claims.FirstOrDefault(c => c.Type == "contactid")?.Value;
            Console.WriteLine("contactId=> " + contactId);
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(contactId))
                return false;
            return await _unitOfWork.Comprep.IsContactExistsAsync(contactId ?? email);
        }

        public async Task<GeneralResult<ComplaintDto>> GetCaseByIdAsync(string id)
        {
            var result = await _unitOfWork.Comprep.GetCaseByIdAsyncc(id);
            if (!result.Status)
                return new GeneralResult<ComplaintDto>
                {
                    Status = false,
                    Errors = result.Errors
                };

            return new GeneralResult<ComplaintDto>
            {
                Status = true,
                Data = new ComplaintDto
                {
                    ComplaintId = result.Data.ComplaintId.ToString(),
                    Name = result.Data.Name,
                    Description = result.Data.Description,
                    Nationalid = result.Data.Nationalid,
                    Location = result.Data.Location,
                    CategoryBinding = result.Data.CategoryBinding,
                    Email = result.Data.Email,
                    TicketNumber = result.Data.TicketNumber,
                    StatusCode = result.Data.StatusCode,
                    CustomerBinding = result.Data.CustomerBinding
                }
            };
        }

        public async Task<GeneralResult<ComplaintDto>> GetCaseByTicketNumberAsync(string ticketNumber)
        {
            var result = await _unitOfWork.Comprep.GetCaseByTicketNumberAsyncc(ticketNumber);
            if (result.Status)
                return new GeneralResult<ComplaintDto>
                {
                    Status = true,
                    Data = new ComplaintDto
                    {
                        ComplaintId = result.Data.ComplaintId.ToString(),
                        Name = result.Data.Name,
                        Description = result.Data.Description,
                        Nationalid = result.Data.Nationalid,
                        Location = result.Data.Location,
                        CategoryBinding = result.Data.CategoryBinding,
                        Email = result.Data.Email,
                        TicketNumber = result.Data.TicketNumber,
                        StatusCode = result.Data.StatusCode,
                        CustomerBinding = result.Data.CustomerBinding
                    }
                };
            return new GeneralResult<ComplaintDto>
            {
                Status = false,
                Errors = result.Errors
            };

        }

        public async Task<GeneralResult<ComplaintDto>> GetCaseByTicketNumberAndEmailAsyncc(string ticketNumber, string email)
        {
            var result = await _unitOfWork.Comprep.GetCaseByTicketNumberAndEmailAsyncc(ticketNumber, email);
            if (result.Status)
                return new GeneralResult<ComplaintDto>
                {
                    Status = true,
                    Data = new ComplaintDto
                    {
                        ComplaintId = result.Data.ComplaintId.ToString(),
                        Name = result.Data.Name,
                        Description = result.Data.Description,
                        Nationalid = result.Data.Nationalid,
                        Location = result.Data.Location,
                        CategoryBinding = result.Data.CategoryBinding,
                        Email = result.Data.Email,
                        TicketNumber = result.Data.TicketNumber,
                        StatusCode = result.Data.StatusCode,
                        CustomerBinding = result.Data.CustomerBinding
                    }
                };
            return new GeneralResult<ComplaintDto>
            {
                Status = false,
                Errors = result.Errors
            };
        }

        public async Task<GeneralResult<List<ComplaintDto>>> GetCasesByCustomerAsync(string customerId)
        {
            var result = await _unitOfWork.Comprep.GetCasesByCustomerAsyncc(customerId);
            if (!result.Status)
                return new GeneralResult<List<ComplaintDto>>
                {
                    Status = false,
                    Errors = result.Errors
                };
            if (result.Data == null || !result.Data.Any())
                return new GeneralResult<List<ComplaintDto>>
                {
                    Status = false,
                    Errors = new[] { new ResultError { Code = "404", Message = "No cases found for the specified customer." } }
                };

            return new GeneralResult<List<ComplaintDto>>
            {
                Status = true,
                Data = result.Data.Select(complaint => new ComplaintDto
                {
                    ComplaintId = complaint.ComplaintId.ToString(),
                    Name = complaint.Name,
                    Description = complaint.Description,
                    Nationalid = complaint.Nationalid,
                    Location = complaint.Location,
                    CategoryBinding = complaint.CategoryBinding,
                    Email = complaint.Email,
                    TicketNumber = complaint.TicketNumber,
                    StatusCode = complaint.StatusCode,
                    CustomerBinding = complaint.CustomerBinding
                }).ToList()
            };

        }


        public async Task<GeneralResult<string>> SubmitComplaintANYNOUMS(ComplaintDto complaint, string email)
        {
            string contactid;
            var existingContact = await _unitOfWork.regrep.GetContactByEmailAsync(email);
            if (existingContact.Data != null && existingContact.Data.Contactid != Guid.Empty)
            {
                contactid = existingContact.Data.Contactid.ToString();
            }
            else
            {
                var newRegister = new Register
                {
                    Email = email,
                    Firstname = null,
                    Lastname = "ANYNOUMS",
                    Password = null,
                    IsAnynoums = true,
                    Gender = 1

                };
                var createdId = await _unitOfWork.regrep.CreateContactAsync(newRegister);
                Console.WriteLine(createdId + "createdId");
                if (createdId.Status)
                    contactid = createdId.Data.ToString();
                else
                    return new GeneralResult<string>
                    {
                        Status = false,
                        Errors = createdId.Errors
                    };
            }
            var com = new Complaint { ComplaintId = Guid.NewGuid(), Email = complaint.Email, Name = complaint.Name, Nationalid = complaint.Nationalid, Location = complaint.Location, Description = complaint.Description, CategoryBinding = complaint.CategoryBinding, CustomerBinding = $"/contacts({contactid})" };
            var result = await _unitOfWork.Comprep.CreateComplaintAsync(com);
            return result;
        }

        public async Task<GeneralResult<string>> DeleteComplaintAsync(string complaintId)
        {
            var result = await _unitOfWork.Comprep.DeleteComplaintAsync(complaintId);
            return result;
        }
    }


}
