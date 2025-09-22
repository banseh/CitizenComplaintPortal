using Citizen_Complaint.BL;
using Citizen_Complaint.BL.Common;
using Citizen_Complaint.BL.Managers.CategoryManager;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Citizen_Complaint.Controllers.CategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager categoryManager;
        public CategoryController(ICategoryManager categoryManager)
        {
            this.categoryManager = categoryManager;
        }
        
        [HttpGet]
        public async Task<Results<Ok<GeneralResult<List<ShowCategoryDto>>>, NotFound<GeneralResult<List<ShowCategoryDto>>>>> GetContactInfoFromToken()
        {
            var result = await categoryManager.GetAllCategory();
            if (result.Status)
                return TypedResults.Ok(result);
            return TypedResults.NotFound(result);
        }
    }
}
