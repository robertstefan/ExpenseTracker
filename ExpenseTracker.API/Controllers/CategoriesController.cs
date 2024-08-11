using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
  [Route("api/categories")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    [HttpGet]
    [Route("list-all")]
    public async Task<ActionResult<List<Category>>> GetAllCategories()
    {
      return (await _categoryService.GetAllCategoriesAsync()).ToList();
    }

    [HttpGet]
    [Route(":id")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
      var category = await _categoryService.GetCategoryByIdAsync(id);

      if (category == null)
      {
        return NotFound();
      }

      return Ok(category);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<int>> AddCategory(Category category)
    {
      // No need for dto since i am not using the user given id in the repo, just ignoring it
      if (category == null )
      {
        return BadRequest();
      }
      if (string.IsNullOrWhiteSpace(category.Name))
      {
        return BadRequest("Category name cannot be empty");
      }

      int categoryId;

      try
      {
        categoryId = await _categoryService.AddCategoryAsync(category);
      }
      catch (Exception ex)
      {
        // @TODO - LOG THE ERROR
        Console.WriteLine(ex);
        return BadRequest("Could not create the category");
      }

      return Ok(new IdOnlyResponse() { Id = categoryId });
    }

    [HttpPut]
    [Route("update")]
    public async Task<ActionResult<Category>> UpdateCategory(Category category)
    {
      if (category == null)
      {
        return BadRequest();
      }
      if (string.IsNullOrWhiteSpace(category.Name))
      {
        return BadRequest("Category name cannot be empty");
      }

      var updatedCategory = await _categoryService.UpdateCategoryAsync(category);

      if (updatedCategory == null)
      {
        return NotFound();
      }

      return Ok(updatedCategory);
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
      var success = await _categoryService.DeleteCategoryAsync(id);

      if (!success)
      {
        return NotFound();
      }

      return Ok();
    }
  }
}
