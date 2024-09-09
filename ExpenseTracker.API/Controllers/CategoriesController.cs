using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoriesController(CategoryService categoryService, ILogger<CategoriesController> logger)
  : ControllerBase
{
  [HttpGet]
  [Route("list-all")]
  public async Task<ActionResult<List<Category>>> GetAllCategories()
  {
    return (await categoryService.GetAllCategoriesAsync()).ToList();
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<Category>> GetCategory(int id)
  {
    var category = await categoryService.GetCategoryByIdAsync(id);

    if (category == null) return NotFound();

    return Ok(category);
  }

  [HttpPost]
  [Route("create")]
  public async Task<ActionResult<int>> AddCategory(Category category)
  {
    // No need for dto since i am not using the user given id in the repo, just ignoring it
    if (category == null) return BadRequest();
    if (string.IsNullOrWhiteSpace(category.Name)) return BadRequest("Category name cannot be empty");

    int categoryId;

    try
    {
      categoryId = await categoryService.AddCategoryAsync(category);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Could not create the category");
      return BadRequest("Could not create the category");
    }

    return Ok(new IdOnlyResponse { Id = categoryId });
  }

  [HttpPut]
  [Route("update")]
  public async Task<ActionResult<Category>> UpdateCategory(Category category)
  {
    if (category == null) return BadRequest();
    if (string.IsNullOrWhiteSpace(category.Name)) return BadRequest("Category name cannot be empty");

    var updatedCategory = await categoryService.UpdateCategoryAsync(category);

    if (updatedCategory == null) return NotFound();

    return Ok(updatedCategory);
  }

  [HttpDelete]
  [Route("delete/{id}")]
  public async Task<IActionResult> DeleteCategory(int id)
  {
    var success = await categoryService.DeleteCategoryAsync(id);

    if (!success) return NotFound();

    return Ok();
  }
}