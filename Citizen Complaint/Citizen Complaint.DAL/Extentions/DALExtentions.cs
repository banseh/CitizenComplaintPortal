using Citizen_Complaint.DAL.Azure_Context;
using Citizen_Complaint.DAL.Reposatory.CompalintRepo;
using Citizen_Complaint.DAL.Reposatory.LoginRepo;
using Citizen_Complaint.DAL.Reposatory.RegisterRepo;
using Citizen_Complaint.DAL.Unit_OfWork;
using Microsoft.Extensions.DependencyInjection;
namespace Citizen_Complaint.DAL.Extentions
{
    public static class DALExtention
    {
        public static void AddDALExtention(this IServiceCollection services)
        {
            services.AddScoped<Azure>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IRegReposatory, RegisterReposatory>();
            services.AddScoped<ILoginReposatory, LoginReposatory>();
            services.AddScoped<IUnitofWok, UnitofWok>();
            services.AddScoped<ICategoryReposatory, CategoryReposatory>();
        }
    }
}
