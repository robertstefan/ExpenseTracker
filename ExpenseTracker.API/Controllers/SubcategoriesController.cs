using ExpenseTracker.API.Common.Extensions;
using ExpenseTracker.API.Common.Options;
using ExpenseTracker.API.Common.Pagination;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Subcategories;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace ExpenseTracker.API.Controllers;


[Route("api/subcategories")]
public class SubcategoriesController : ApiController
{
    private readonly SubcategoryService _subcategoryService;
    private readonly SoftDeleteSettings _softDeleteSettings;

    public SubcategoriesController(SubcategoryService subcategoryService, IOptions<SoftDeleteSettings> softDeleteSettings)
    {
        _subcategoryService = subcategoryService;
        _softDeleteSettings = softDeleteSettings.Value;
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> AddSubcategoryAsync([FromBody] CreateSubcategoryRequest request)
    {
        var validationErrors = request.GetValidationErrors();

        if (validationErrors.Count != 0)
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            var subcategory = Subcategory.CreateNew(request.Name, request.CategoryId);

            Guid subcategoryId = await _subcategoryService.AddSubcategoryAsync(subcategory);

            if (subcategoryId != Guid.Empty)
            {
                Log.Information("Subcategory with the id {id} was created.");
                return subcategoryId;
            }

            Log.Error("Subcategory could not be added.");

            return BadRequest("Subcategory could not be added");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Subcategory could not be added.");

            return BadRequest("Subcategory could not be added.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubcategoryDTO>> GetSubcategoryByIdAsync(Guid Id)
    {
        if (Id.IsEmpty())
        {
            return BadRequest("The id cannot be empty");
        }
        try
        {
            Subcategory? subcategory = await _subcategoryService.GetSubcategoryByIdAsync(Id);

            if (subcategory == null)
            {
                Log.Error("No subcategory with the id {id} was found", Id);

                return NotFound("No subcategories with the associated id were found.");
            }

            return new SubcategoryDTO(subcategory);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Subcategory with the id {id} could not be retrieved", Id);

            return BadRequest("Subcategory with could not be retrieved");
        }
    }

    [HttpGet]
    public async Task<ActionResult<Paged<SubcategoryDTO>>> GetSubcategoriesPaginatedAsync([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 10)
    {
        try
        {
            var subcategories = await _subcategoryService.GetSubcategoriesPaginatedAsync(PageNumber, PageSize);

            return new Paged<SubcategoryDTO>(subcategories.TotalCount, PageNumber, PageSize)
            {
                Items = subcategories.Rows.Select(subcategory => new SubcategoryDTO(subcategory))
            };

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Subcategories could not be retrieved");

            return BadRequest("Subcategories could not be retrieved");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSubcategoryAsync(Guid Id, [FromBody] UpdateSubcategoryRequest request)
    {
        var validationErrors = request.GetValidationErrors();

        if (Id.IsEmpty())
        {
            return BadRequest("The id cannot be empty");
        }

        if (validationErrors.Count != 0)
        {
            return BadRequest(new { Errors = validationErrors });
        }
        try
        {
            var subcategory = Subcategory.Create(
            Id,
            request.Name,
            request.CategoryId
        );

            bool updateSuccess = await _subcategoryService.UpdateSubcategoryAsync(subcategory);

            if (updateSuccess)
            {
                Log.Information("Subcategory with the {id} was updated.", Id);
                return Ok();
            }

            Log.Error("Subcategory with the {id} could not be updated.");

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Subcategory with the {id} could not be updated", Id);
            return BadRequest("Subcategory could not be updated");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSubcategoryAsync(Guid Id)
    {
        if (Id.IsEmpty())
        {
            return BadRequest("The id cannot be empty");
        }

        try
        {
            var softDelete = _softDeleteSettings.SoftDelete;
            var deleteSuccess = await _subcategoryService.DeleteSubcategoryAsync(Id, softDelete);

            if (deleteSuccess)
            {
                Log.Information("Subcategory with the {id} was deleted.", Id);

                return Ok();
            }
            Log.Error("Subcategory with the {id} could not be deleted.");

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Subcategory with the {id} could not be deleted.", Id);

            return BadRequest("Subcategory could not be deleted");
        }
    }
}
