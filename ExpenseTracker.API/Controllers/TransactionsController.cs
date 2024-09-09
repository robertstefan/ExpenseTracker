using System.Security.Claims;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionsController(
  TransactionService transactionService,
  ICurrencyExchangeProvider _exchangeRateProvider,
  ILogger<TransactionsController> logger)
  : ControllerBase
{
  [HttpGet]
  [Route("list-all")]
  public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
  {
    return (await transactionService.GetAllTransactionsAsync()).ToList();
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
  {
    var transaction = await transactionService.GetTransactionByIdAsync(id);

    if (transaction == null) return NotFound();

    return Ok(transaction);
  }

  [HttpGet]
  [Route("get-by-type/:type")]
  public async Task<ActionResult<IEnumerable<Transaction>>> GetByType(TransactionType type)
  {
    return (await transactionService.GetTransactionsByTypeAsync(type)).ToList();
  }

  [Authorize]
  [HttpPost]
  [Route("create")]
  public async Task<ActionResult<Transaction>> AddTransaction(TransactionDTO transactionModel)
  {
    // @TODO: error on categoryId/subcategoryId being invalid
    if (transactionModel == null) return BadRequest();

    if (transactionModel.Amount <= 0) return BadRequest("Amount cannot be less or equal to 0");

    if (transactionModel.Date <= DateTime.MinValue)
      return BadRequest($"Date cannot be lower than ${DateTime.MinValue.Date.ToShortDateString()}");

    // Extract the UserId from the JWT claims
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
      return Unauthorized("Invalid user");
    Guid result;

    try
    {
      var exchangeRate = _exchangeRateProvider[transactionModel.Currency] != 0
        ? _exchangeRateProvider[transactionModel.Currency]
        : 1.0;
      result = await transactionService.AddTransactionAsync(new Transaction
      {
        Amount = transactionModel.Amount,
        CategoryId = transactionModel.CategoryId,
        SubcategoryId = transactionModel.SubcategoryId,
        Date = transactionModel.Date,
        Description = transactionModel.Description,
        IsRecurrent = transactionModel.IsRecurrent,
        UserId = int.Parse(userId),
        Type = transactionModel.Type,
        Currency = transactionModel.Currency,
        ExchangeRate = exchangeRate
      });
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Could not register the transaction");
      return BadRequest("Could not register the transaction");
    }

    return Ok(new IdOnlyResponse { Id = result });
  }

  [HttpPut]
  [Route("update")]
  public async Task<ActionResult<Transaction>> UpdateTransaction(Transaction transaction)
  {
    if (transaction == null) return BadRequest();

    var updated = await transactionService.UpdateTransactionAsync(transaction);
    // Repo returns null if no transaction is found
    if (updated == null) return NotFound();
    return Ok(updated);
  }

  [HttpDelete]
  [Route("delete/{id}")]
  public async Task<IActionResult> DeleteTransaction(Guid Id)
  {
    var succes = await transactionService.DeleteTransactionAsync(Id);
    if (!succes) return NotFound();

    return Ok();
  }
}