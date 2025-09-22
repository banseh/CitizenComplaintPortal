using Citizen_Complaint.DAL.Azure_Context;
using Citizen_Complaint.DAL.Reposatory.CompalintRepo;
using Citizen_Complaint.DAL.Reposatory.LoginRepo;
using Citizen_Complaint.DAL.Reposatory.RegisterRepo;

namespace Citizen_Complaint.DAL.Unit_OfWork
{
    public class UnitofWok : IUnitofWok
    {
        public IComplaintRepository Comprep { get; }
        public ILoginReposatory logrep { get; }
        public IRegReposatory regrep { get; }
        public ICategoryReposatory categoryReposatory { get; }
        private readonly Azure azure;
        public UnitofWok(IComplaintRepository _Comprep,
             ILoginReposatory _logrep,
             IRegReposatory _regrep,
             Azure _azure,
             ICategoryReposatory categoryReposatory
        )
        {
            Comprep = _Comprep;
            logrep = _logrep;
            regrep = _regrep;
            azure = _azure;
            this.categoryReposatory = categoryReposatory;
        }
    }
}
