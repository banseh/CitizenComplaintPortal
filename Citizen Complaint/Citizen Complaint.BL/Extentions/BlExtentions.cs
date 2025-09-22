using Citizen_Complaint.BL.Managers.CategoryManager;
using Citizen_Complaint.BL.Managers.CompalinManager;
using Citizen_Complaint.BL.Managers.JWTManager;
using Citizen_Complaint.BL.Managers.LoginManager;
using Citizen_Complaint.BL.Managers.RegisterManager;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Citizen_Complaint.BL.Extentions
{
    public static class BLExtention
    {
        public static void AddBLExtention(this IServiceCollection services)
        {
            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddScoped<IRegisterService, RegisterService>();
          
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ICategoryManager, CategoryManager>();
           
            services.AddValidatorsFromAssembly(typeof(BLExtention).Assembly);
            services.AddScoped<IExtractContactInfoFromToken, ExtractContactInfoFromToken>();
        }
    }
}
