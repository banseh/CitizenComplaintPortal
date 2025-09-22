using Citizen_Complaint.BL.Common;
using Citizen_Complaint.DAL.Unit_OfWork;

namespace Citizen_Complaint.BL.Managers.CategoryManager
{
    public class CategoryManager : ICategoryManager
    {
        public IUnitofWok unitofWok { get; }
        public CategoryManager(IUnitofWok unitofWok)
        {
            this.unitofWok = unitofWok;
        }

        public async Task<GeneralResult<List<ShowCategoryDto>>> GetAllCategory()
        {
            var result = await unitofWok.categoryReposatory.GetAllCategoriesAsync();
            if (!result.Status) return new GeneralResult<List<ShowCategoryDto>>
            {
                Status = false,
                Errors = result.Errors,
            };
            var categories =
                result.Data.Select(c => new ShowCategoryDto { CategoryId = c.CategoryId, Description = c.Description, Name = c.Name }).ToList();
            return new GeneralResult<List<ShowCategoryDto>>
            {
                Status = true,
                Data = categories
            };
        }
    }
}
