using ExpenseTracker.API.DTOs.Categories;

using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _categoriesService;
        private readonly TransactionsService _transactionsService;

        public CategoriesController(CategoriesService categoriesService, TransactionsService transactionsService)
        {
            _categoriesService = categoriesService;
            _transactionsService = transactionsService;

        }


        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(Guid categoryId)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(categoryId);

            if (category == null)
            {
                return BadRequest("Resource not found");
            }

            try
            {
                return Ok(new CategoryDTO(category));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving the category.");
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoriesService.GetAllCategoriesAsync();
                List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
                foreach (var category in categories)
                {
                    categoryDTOs.Add(new CategoryDTO(category));
                }
                return Ok(categoryDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving categories.");
            }
        }


        [HttpGet("{categoryId}/transactions")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByCategory(Guid categoryId)
        {
            try
            {
                var transactions = await _transactionsService.GetTransactionsByCategoryIdAsync(categoryId);
                List<TransactionDTO> transactionDTOs = new List<TransactionDTO>();
                foreach (var transaction in transactions)
                {
                    transactionDTOs.Add(new TransactionDTO(transaction));
                }
                return Ok(transactionDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving transactions");
            }

        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CreateCategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest("Payload can't be null");
            }

            if (string.IsNullOrEmpty(categoryDTO.CategoryName))
            {
                return BadRequest("Category name can't be null or empty");
            }

            try
            {
                Category categoryToCreate = new Category() { CategoryName = categoryDTO.CategoryName };

                Guid createdCategoryId = await _categoriesService.CreateCategoryAsync(categoryToCreate);

                return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategoryId }, categoryToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while creating the category.");
            }
        }

        [HttpPost("{parentCategoryId}/create-subcategory")]
        public async Task<ActionResult<Category>> CreateSubCategory(Guid parentCategoryId, CreateCategoryDTO categoryDTO)
        {
            var parentCategory = await _categoriesService.GetCategoryByIdAsync(parentCategoryId);


            if (parentCategory == null)
            {
                return BadRequest("Parent Category with given id does not exist");
            }

            if (parentCategory.ParentCategoryId != null)
            {
                return BadRequest("You can't create a subcategory for this category because it's already a subcategory.");
            }

            if (categoryDTO == null)
            {
                return BadRequest("Payload can't be null");
            }

            if (string.IsNullOrEmpty(categoryDTO.CategoryName))
            {
                return BadRequest("Category name can't be null or empty");
            }

            try
            {
                Category categoryToCreate = new Category() { CategoryName = categoryDTO.CategoryName, ParentCategoryId = parentCategoryId, ParentCategory = parentCategory };

                Guid createdCategoryId = await _categoriesService.CreateCategoryAsync(categoryToCreate);

                return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategoryId }, categoryToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while creating the subcategory");
            }
        }


        [HttpPost("{categoryId}/transactions")]
        public async Task<ActionResult<Guid>> CreateTransaction(CreateTransactionDTO transactionModel, Guid categoryId)
        {

            if (transactionModel == null)
            {
                return BadRequest();
            }

            if (transactionModel.Amount <= 0)
            {
                return BadRequest("Amount cannot be less or equal to 0");
            }

            if (transactionModel.Date <= DateTime.MinValue)
            {
                return BadRequest($"Date cannot be lower than ${DateTime.MinValue.Date.ToShortDateString()}");
            }

            var category = await _categoriesService.GetCategoryByIdAsync(categoryId);


            try
            {
                Transaction transactionToCreate = new Transaction()
                {
                    Amount = transactionModel.Amount,
                    Date = transactionModel.Date,
                    Description = transactionModel.Description,
                    IsRecurrent = transactionModel.IsRecurrent,
                    TransactionType = transactionModel.TransactionType,
                    CategoryId = categoryId,
                    UserId = transactionModel.UserId
                };

                Guid createdTransactionId = await _transactionsService.AddTransactionAsync(transactionToCreate);

                return Created($"https://localhost:7032/api/transactions/{createdTransactionId}", transactionToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while creating the transaction");
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateCategory(Guid categoryId, UpdateCategoryDTO updateCategoryDTO)
        {
            Category categoryToUpdate = await _categoriesService.GetCategoryByIdAsync(categoryId);

            if(categoryToUpdate == null)
            {
                return BadRequest("Resource not found");
            }

            try
            {
                categoryToUpdate.CategoryName = updateCategoryDTO.CategoryName;
                await _categoriesService.UpdateCategoryAsync(categoryToUpdate);
                return NoContent();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while updating the category");
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory(Guid categoryId)
        {
            bool isSuccesfullyDeleted = await _categoriesService.DeleteCategoryAsync(categoryId);

            return isSuccesfullyDeleted ? NoContent() : BadRequest();
        }
    }
}
