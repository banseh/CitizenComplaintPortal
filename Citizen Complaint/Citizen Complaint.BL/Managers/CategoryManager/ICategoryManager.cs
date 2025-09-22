using Citizen_Complaint.BL.Common;

namespace Citizen_Complaint.BL.Managers.CategoryManager
{
    public interface ICategoryManager
    {
        public Task<GeneralResult<List<ShowCategoryDto>>> GetAllCategory();
    }
}
