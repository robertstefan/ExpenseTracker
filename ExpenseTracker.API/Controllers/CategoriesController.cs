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

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory(Guid categoryId)
        {
            bool isSuccesfullyDeleted = await _categoriesService.DeleteCategoryAsync(categoryId);

            return isSuccesfullyDeleted ? NoContent() : BadRequest();
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetCategoryById(Guid categoryId)
        {
            return Ok(await _categoriesService.GetCategoryByIdAsync(categoryId));
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            return Ok(await _categoriesService.GetAllCategoriesAsync());
        }

        [HttpPost("{categoryId}/transactions")]
        public async Task<ActionResult<Transaction>> CreateTransaction(CreateTransactionDTO transactionModel, Guid categoryId)
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

            try
            {
                Transaction transactionToCreate = new Transaction()
                {
                    Amount = transactionModel.Amount,
                    Date = transactionModel.Date,
                    Description = transactionModel.Description,
                    IsRecurrent = transactionModel.IsRecurrent,
                    TransactionType = transactionModel.TransactionType,
                    CategoryId = categoryId
                };

                Guid createdTransactionId = await _transactionsService.AddTransactionAsync(transactionToCreate);

                return CreatedAtAction(nameof(GetTransactionByIdAndCategoryId), new { categoryId = categoryId, transactionId = createdTransactionId }, transactionToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{categoryId}/transactions/{transactionId}")]
        public async Task<ActionResult<Transaction>> GetTransactionByIdAndCategoryId(Guid categoryId, Guid transactionId)
        {
            return Ok(await _transactionsService.GetTransactionByIdAndCategoryIdAsync(transactionId, categoryId));
        }

    }
}
