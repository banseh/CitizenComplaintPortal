using Citizen_Complaint.DAL.Reposatory.CompalintRepo;
using Citizen_Complaint.DAL.Reposatory.LoginRepo;
using Citizen_Complaint.DAL.Reposatory.RegisterRepo;

namespace Citizen_Complaint.DAL.Unit_OfWork
{
    public interface IUnitofWok
    {
        public IComplaintRepository Comprep { get; }
        public ILoginReposatory logrep { get; }
        public IRegReposatory regrep { get; }
        public ICategoryReposatory categoryReposatory { get; }
    }
}
