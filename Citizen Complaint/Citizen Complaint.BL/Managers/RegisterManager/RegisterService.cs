using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Dtos.ContactDto;
using Citizen_Complaint.BL.Dtos.RegisterDto;
using Citizen_Complaint.DAL;
using Citizen_Complaint.DAL.Unit_OfWork;
using FluentValidation;

namespace Citizen_Complaint.BL.Managers.RegisterManager
{
    public class RegisterService : IRegisterService
    {
        private readonly IUnitofWok _unitOfWork;
        private readonly IValidator<Register> validator;
        public RegisterService(IUnitofWok unitOfWork, IValidator<Register> validator)
        {
            _unitOfWork = unitOfWork;
            this.validator = validator;
        }
        public async Task<GeneralResult<string>> CreateContactAsync(Register register)
        {
            var validationResult = await validator.ValidateAsync(register);
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
            return await _unitOfWork.regrep.CreateContactAsync(register);
        }

        public async Task<GeneralResult<RegisterDTO_s>> GetContactByEmailAsync(string mail)
        {
            var registerRes = await _unitOfWork.regrep.GetContactByEmailAsync(mail);
            if (!registerRes.Status)
                return new GeneralResult<RegisterDTO_s>
                {
                    Status = false,
                    Errors = registerRes.Errors

                };
            var register = registerRes.Data;
            var registerDTO_s = new RegisterDTO_s { Email = register.Email, Firstname = register.Firstname, Lastname = register.Lastname, Gender = register.Gender, IsAnynoums = register.IsAnynoums, Password = register.Password };
            return new GeneralResult<RegisterDTO_s>
            {
                Status = true,
                Data = registerDTO_s
            };
        }
        public async Task<GeneralResult<string>> CreateOrUpdateContactAsync(Register register)
        {

            var validationResult = await validator.ValidateAsync(register);
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
            var existingContact = await GetContactByEmailAsync(register.Email);
            Console.WriteLine("existingContact=> " + existingContact);
            Console.WriteLine("register.Email=> " + register.Email);
            if (existingContact.Data != null)
            {
                Console.WriteLine("iiiiiiiiii" + existingContact.Data.IsAnynoums);
                if (existingContact.Data.IsAnynoums)
                {
                    var contact = await _unitOfWork.regrep.GetContactByEmailAsync(register.Email);
                    Console.WriteLine(" contact.Data.Email=> " + contact.Data.Email);
                    var contactId = contact.Data.Contactid.ToString(); 
                    contact.Data.Lastname = existingContact.Data.Lastname;
                    Console.WriteLine("register.Lastname=> " + register.Lastname);
                    return await UpdateContactAsync(contactId, register);
                }
                else
                {
                    return new GeneralResult<string>
                    {
                        Status = false,
                        Errors = new ResultError[]
                        {
                            new ResultError{Code = "400",Message = "Email already in use by a registered user"}
                        }
                    };
                }
            }
            else
            {
                return await CreateContactAsync(register);
            }
        }
        public async Task<GeneralResult<string>> UpdateContactAsync(string contactId, Register reg)
        {
            Console.WriteLine("reg.Lastname=> " + reg.Lastname);
            return await _unitOfWork.regrep.UpdateAnonymousContactAsync(contactId, reg);
        }
        public async Task<GeneralResult<ContectFromToken>> getcontactidfromtoken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var contactId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"Extracted from token: Email = {email}, ContactId = {contactId}");
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(contactId))
            {
                return new GeneralResult<ContectFromToken>
                {
                    Status = false,
                    Errors = new ResultError[] { new ResultError { Code = "400", Message = "Invalid token: No email or contact ID found." } }
                };
            }

            var exists = await _unitOfWork.Comprep.IsContactExistsAsync(contactId ?? email);
            if (!exists)
            {
                return new GeneralResult<ContectFromToken>
                {
                    Status = false,
                    Data = new ContectFromToken
                    {
                        Email = email,
                        ContactId = contactId
                    },
                    Errors = new ResultError[] { new ResultError { Code = "404", Message = "No matching contact found." } }
                };
            }

            return new GeneralResult<ContectFromToken>
            {
                Errors = null,
                Status = true,
                Data = new ContectFromToken
                {
                    Email = email,
                    ContactId = contactId
                }
            };

        }
    }
}
