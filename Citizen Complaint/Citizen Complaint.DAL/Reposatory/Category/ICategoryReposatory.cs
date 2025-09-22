using Citizen_Complaint.BL.Common;

namespace Citizen_Complaint.DAL
{
    public interface ICategoryReposatory
    {
        public Task<GeneralResult<List<Category>>> GetAllCategoriesAsync();
    }
}
