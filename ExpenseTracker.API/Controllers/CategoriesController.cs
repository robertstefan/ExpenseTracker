using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Categories;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CategoryService _categoryService, ILogger<CategoriesController> _logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(CreateCategoryRequest request)
    {
        var category = Category.CreateNew(
            request.Name
        );

        try
        {
            var result = await _categoryService.AddCategoryAsync(category);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest();
        }
    }

    [HttpGet("list")]
    public async Task<ActionResult<CategoryDTO>> GetTransactionsPaginated([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        List<Category> categories = (await _categoryService.GetCategoriesPaginatedAsync(offset, limit)).ToList();

        if (categories == null)
        {
            return Ok(Enumerable.Empty<CategoryDTO>());
        }

        return Ok(categories.Select(category => new CategoryDTO(category)).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(string id)
    {

        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(new CategoryDTO(category));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Could not retrieve the category");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> UpdateCategory(Guid id, UpdateCategoryRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        var category = Category.Create(
            id,
            request.Name
        );

        try
        {
            await _categoryService.UpdateCategoryAsync(category);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Could not update the category.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await _categoryService.DeleteCategoryAsync(id);

        return NoContent();
    }

}
