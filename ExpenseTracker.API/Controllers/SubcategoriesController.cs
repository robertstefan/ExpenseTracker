using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Route("api/subcategories")]
[ApiController]
public class SubcategoriesController(SubcategoryService subcategoryService, ILogger<SubcategoriesController> logger)
  : ControllerBase
{
  [HttpGet]
  [Route("list-all")]
  public async Task<ActionResult<List<Subcategory>>> GetAllSubcategories()
  {
    return (await subcategoryService.GetAllSubcategoriesAsync()).ToList();
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<Subcategory>> GetSubcategory(int id)
  {
    var subcategory = await subcategoryService.GetSubcategoryByIdAsync(id);

    if (subcategory == null) return NotFound();

    return Ok(subcategory);
  }

  [HttpGet]
  [Route("by-category/{categoryId}")]
  public async Task<ActionResult<List<Subcategory>>> GetSubcategoriesByCategoryId(int categoryId)
  {
    var subcategories = await subcategoryService.GetSubcategoriesByCategoryIdAsync(categoryId);

    if (!subcategories.Any()) return NotFound();

    return Ok(subcategories.ToList());
  }

  [HttpPost]
  [Route("create")]
  public async Task<ActionResult<int>> AddSubcategory(Subcategory subcategory)
  {
    if (subcategory == null || string.IsNullOrWhiteSpace(subcategory.Name))
      return BadRequest("Subcategory name cannot be empty");

    int subcategoryId;

    try
    {
      subcategoryId = await subcategoryService.AddSubcategoryAsync(subcategory);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Could not create the subcategory");
      return BadRequest("Could not create the subcategory");
    }

    return Ok(new IdOnlyResponse { Id = subcategoryId });
  }

  [HttpPut]
  [Route("update")]
  public async Task<ActionResult<Subcategory>> UpdateSubcategory(Subcategory subcategory)
  {
    if (subcategory == null || string.IsNullOrWhiteSpace(subcategory.Name))
      return BadRequest("Subcategory name cannot be empty");

    var updatedSubcategory = await subcategoryService.UpdateSubcategoryAsync(subcategory);

    if (updatedSubcategory == null) return NotFound();

    return Ok(updatedSubcategory);
  }

  [HttpDelete]
  [Route("delete/{id}")]
  public async Task<IActionResult> DeleteSubcategory(int id)
  {
    var success = await subcategoryService.DeleteSubcategoryAsync(id);

    if (!success) return NotFound();

    return Ok();
  }
}