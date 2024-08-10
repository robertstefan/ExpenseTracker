using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
  [Route("api/transactions")]
  [ApiController]
  public class TransactionsController : ControllerBase
  {
    private readonly TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService)
    {
      _transactionService = transactionService;
    }

    [HttpGet]
    [Route("list-all")]
    public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
    {
      return (await _transactionService.GetAllTransactionsAsync()).ToList();
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Transaction>> AddTransaction(TransactionDTO transactionModel)
    {
      if (transactionModel == null)
      {
        return BadRequest();
      }

      if (transactionModel.Amount <= 0)
      {
        return BadRequest("Amount cannot be less or equal to 0");
      }

      if (string.IsNullOrEmpty(transactionModel.Category))
      {
        transactionModel.Category = "Diverse";
      }

      if (transactionModel.Date <= DateTime.MinValue)
      {
        return BadRequest($"Date cannot be lower than ${DateTime.MinValue.Date.ToShortDateString()}");
      }

      Guid result;

      try
      {
        result = await _transactionService.AddTransactionAsync(new Transaction()
        {
          Amount = transactionModel.Amount,
          Category = transactionModel.Category,
          Date = transactionModel.Date,
          Description = transactionModel.Description,
          IsRecurrent = transactionModel.IsRecurrent,
          TransactionType = transactionModel.TransactionType,
        });
      }
      catch (Exception ex)
      {
        // @TODO - LOG THE ERROR
        return BadRequest("Could not register the transaction");
      }

      return Ok(new IdOnlyResponse() { Id = result});
    }

  }
}
