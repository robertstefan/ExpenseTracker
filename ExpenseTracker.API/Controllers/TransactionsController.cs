using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsService _transactionsService;

        public TransactionsController(TransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            return (await _transactionsService.GetAllTransactionsAsync()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransactionById(Guid id)
        {
            return Ok(await _transactionsService.GetTransactionByIdAsync(id));
        }

        [HttpGet("/search-by-type")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionsByTransactionType(TransactionType transactionType)
        {
            return Ok(await _transactionsService.GetTransactionsByTypeAsync(transactionType));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(Guid id)
        {
            bool isSuccesfullyDeleted = await _transactionsService.DeleteTransactionAsync(id);

            return isSuccesfullyDeleted ? NoContent() : BadRequest();

        }

        [HttpPut("{transactionId}")]
        public async Task<ActionResult> UpdateTransaction(Guid transactionId, UpdateTransactionDTO transactionModel)
        {
            Transaction transactionToUpdate = await _transactionsService.GetTransactionByIdAsync(transactionId);

            if (transactionToUpdate == null)
                return BadRequest();
            if (transactionModel.Amount <= 0)
                return BadRequest("Amount cannot be less or equal to 0");
            if (transactionModel.Date <= DateTime.MinValue)
                return BadRequest("Invalid Date");

            try
            {
                transactionToUpdate.Amount = transactionModel.Amount;
                transactionToUpdate.Description = transactionModel.Description;
                transactionToUpdate.Date = transactionModel.Date;
                transactionToUpdate.IsRecurrent = transactionModel.IsRecurrent;
                transactionToUpdate.TransactionType = transactionModel.TransactionType;
                await _transactionsService.UpdateTransactionAsync(transactionToUpdate);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }

        }

    }
}
