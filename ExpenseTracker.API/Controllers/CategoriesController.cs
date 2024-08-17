using ExpenseTracker.API.Common.Extensions;
using ExpenseTracker.API.Common.Options;
using ExpenseTracker.API.Common.Pagination;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Categories;
using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;
    private readonly SoftDeleteSettings _softDeleteSettings;

    public CategoriesController(CategoryService categoryService, IOptions<SoftDeleteSettings> softDeleteSettings)
    {
        _categoryService = categoryService;
        _softDeleteSettings = softDeleteSettings.Value;
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var validationErrors = request.GetValidationErrors();

        if (validationErrors.Count != 0)
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            var category = Category.CreateNew(
            request.Name
        );

            Guid categoryId = await _categoryService.AddCategoryAsync(category);

            if (categoryId != Guid.Empty)
            {
                Log.Information("Category with the id {id} was created.");
                return categoryId;
            }
            Log.Error("Category could not be added.");

            return BadRequest("Category could not be added");

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Category could not be added.");

            return BadRequest("Category could not be added.");
        }
    }

    [HttpGet("list")]
    public async Task<Paged<CategoryDTO>> GetTransactionsPaginated([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 10)
    {
        var categories = await _categoryService.GetCategoriesPaginatedAsync(PageNumber, PageSize);

        return new Paged<CategoryDTO>(categories.TotalCount, PageNumber, PageSize)
        {
            Items = categories.Rows.Select(category => new CategoryDTO(category))
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
    {
        if (id.IsEmpty())
        {
            return BadRequest("The id cannot be empty");
        }
        try
        {
            Category? category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                Log.Error("No category with the id {id} was found", id);

                return NotFound("No categories with the associated id were found.");
            }

            return new CategoryDTO(category);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Category with the id {id} could not be retrieved", id);

            return BadRequest("Could not retrieve the cateogry");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDTO>> UpdateCategory(Guid id, UpdateCategoryRequest request)
    {
        if (id.IsEmpty())
        {
            return BadRequest("The id cannot be empty");
        }

        var validationErrors = request.GetValidationErrors();

        if (validationErrors.Count != 0)
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            var category = Category.Create(
            id,
            request.Name
        );

            bool updateSuccess = await _categoryService.UpdateCategoryAsync(category);

            if (updateSuccess)
            {
                Log.Information("Category with the {id} was updated.", id);
                return Ok();
            }

            Log.Error("Category with the {id} could not be updated.");

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Category with the {id} could not be updated", id);
            return BadRequest("Category could not be updated.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        if (id.IsEmpty())
        {
            return BadRequest("The id cannot be empty");
        }

        try
        {
            var softDelete = _softDeleteSettings.SoftDelete;
            var deleteSuccess = await _categoryService.DeleteCategoryAsync(id, softDelete);

            if (deleteSuccess)
            {
                Log.Information("Category with the {id} was deleted.", id);

                return Ok();
            }
            Log.Error("Category with the {id} could not be deleted.");

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Subcategory with the {id} could not be deleted.", id);

            return BadRequest("Subcategory could not be deleted");
        }
    }

}
