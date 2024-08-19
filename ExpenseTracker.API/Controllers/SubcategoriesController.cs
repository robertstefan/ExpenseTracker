using ExpenseTracker.API.DTOs.Subcategories;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/subcategories")]
    [ApiController]
    public class SubcategoriesController : ControllerBase
    {

        private readonly SubcategoriesService _subcategoriesService;

        public SubcategoriesController(SubcategoriesService subcategoriesService)
        {
            _subcategoriesService = subcategoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubcategoryDTO>>> GetAllSubcategories()
        {
            var subcategories = await _subcategoriesService.GetAllSubcategoriesAsync();

            List<SubcategoryDTO> subcategoriesDTOs = new List<SubcategoryDTO>();

            foreach (var subcategory in subcategories)
            {
                subcategoriesDTOs.Add(new SubcategoryDTO(subcategory));
            }

            return Ok(subcategoriesDTOs);
        }

        [HttpGet("{subcategoryId}")]
        public async Task<ActionResult<SubcategoryDTO>> GetSubcategoryById(Guid subcategoryId)
        {
            var subcategory = await _subcategoriesService.GetSubcategoryByIdAsync(subcategoryId);
            return Ok(new SubcategoryDTO(subcategory));
        }

        [HttpDelete("{subcategoryId}")]
        public async Task<ActionResult> DeleteSubcategory(Guid subcategoryId)
        {
            var isSuccesfullyDeleted = await _subcategoriesService.DeleteSubcategoryAsync(subcategoryId);

            return isSuccesfullyDeleted ? NoContent() : NotFound("Subcategory not found");
        }

    }
}
