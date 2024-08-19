using ExpenseTracker.API.DTOs.Categories;
using ExpenseTracker.API.DTOs.Subcategories;
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
        private readonly SubcategoriesService _subcategoriesService;

        public CategoriesController(CategoriesService categoriesService, TransactionsService transactionsService, SubcategoriesService subcategoriesService)
        {
            _categoriesService = categoriesService;
            _transactionsService = transactionsService;
            _subcategoriesService = subcategoriesService;
        }


        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(Guid categoryId)
        {
            try
            {
                var category = await _categoriesService.GetCategoryByIdAsync(categoryId);
                return Ok(new CategoryDTO(category));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
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
                return StatusCode(500);
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
                return StatusCode(500);
            }

        }


        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(categoryDTO.CategoryName))
            {
                return BadRequest();
            }

            try
            {
                Category categoryToCreate = new Category() { CategoryName = categoryDTO.CategoryName };

                Guid createdCategoryId = await _categoriesService.CreateCategoryAsync(categoryToCreate);

                return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategoryId }, categoryToCreate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

            if (category.Subcategory == null)
            {
                return BadRequest("You can't insert a transaction into a category that doesn't have a subcategory.");
            }

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
                    SubcategoryId = category.Subcategory.Id
                };

                Guid createdTransactionId = await _transactionsService.AddTransactionAsync(transactionToCreate);

                return Created($"https://localhost:7032/api/transactions/{createdTransactionId}", transactionToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }


        [HttpPost("{categoryId}/subcategories")]
        public async Task<ActionResult<Guid>> CreateSubcategory(CreateSubcategoryDTO subcategoryDTO, Guid categoryId)
        {
            try
            {
                Subcategory subcategoryToCreate = new Subcategory() { SubcategoryName = subcategoryDTO.SubcategoryName, CategoryId = categoryId };
                Guid createdSubcategoryId = await _subcategoriesService.CreateSubcategoryAsync(subcategoryToCreate);

                return createdSubcategoryId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
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
