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
        public async Task<ActionResult<List<TransactionSummaryDTO>>> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionsService.GetAllTransactionsAsync();
                List<TransactionSummaryDTO> transactionSummaries = new List<TransactionSummaryDTO>();
                foreach (var transaction in transactions)
                {
                    transactionSummaries.Add(new TransactionSummaryDTO(transaction));
                }
                return Ok(transactionSummaries);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }


        [HttpGet("{transactionId}")]
        public async Task<ActionResult<TransactionSummaryDTO>> GetTransactionById(Guid transactionId)
        {
            try
            {
                var transaction = await _transactionsService.GetTransactionByIdAsync(transactionId);

                return Ok(new TransactionSummaryDTO(transaction));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }


        [HttpGet("/search-by-type")]
        public async Task<ActionResult<List<TransactionSummaryDTO>>> GetTransactionsByTransactionType(TransactionType transactionType)
        {
            try
            {
                var transactions = await _transactionsService.GetTransactionsByTypeAsync(transactionType);
                List<TransactionSummaryDTO> transactionSummaries = new List<TransactionSummaryDTO>();
                foreach (var transaction in transactions)
                {
                    transactionSummaries.Add(new TransactionSummaryDTO(transaction));
                }

                return transactionSummaries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }


        [HttpDelete("{transactionId}")]
        public async Task<ActionResult> DeleteTransaction(Guid transactionId)
        {
            bool isSuccesfullyDeleted = await _transactionsService.DeleteTransactionAsync(transactionId);

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
