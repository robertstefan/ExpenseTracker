using System.Security.Claims;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

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

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
  {
    var transaction = await _transactionService.GetTransactionByIdAsync(id);

    if (transaction == null) return NotFound();

    return Ok(transaction);
  }

  [HttpGet]
  [Route("get-by-type/:type")]
  public async Task<ActionResult<IEnumerable<Transaction>>> GetByType(TransactionType type)
  {
    return (await _transactionService.GetTransactionsByTypeAsync(type)).ToList();
  }

  [Authorize]
  [HttpPost]
  [Route("create")]
  public async Task<ActionResult<Transaction>> AddTransaction(TransactionDTO transactionModel)
  {
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
      result = await _transactionService.AddTransactionAsync(new Transaction
      {
        Amount = transactionModel.Amount,
        CategoryId = transactionModel.CategoryId,
        SubcategoryId = transactionModel.SubcategoryId,
        Date = transactionModel.Date,
        Description = transactionModel.Description,
        IsRecurrent = transactionModel.IsRecurrent,
        UserId = int.Parse(userId),
        Type = transactionModel.Type
      });
    }
    catch (Exception ex)
    {
      // @TODO - LOG THE ERROR
      Console.WriteLine(ex);
      return BadRequest("Could not register the transaction");
    }

    return Ok(new IdOnlyResponse { Id = result });
  }

  [HttpPut]
  [Route("update")]
  public async Task<ActionResult<Transaction>> UpdateTransaction(Transaction transaction)
  {
    if (transaction == null) return BadRequest();


    var updated = await _transactionService.UpdateTransactionAsync(transaction);
    // Repo returns null if no transaction is found
    if (updated == null) return NotFound();
    return Ok(updated);
  }

  [HttpDelete]
  [Route("delete/{id}")]
  public async Task<IActionResult> DeleteTransaction(Guid Id)
  {
    var succes = await _transactionService.DeleteTransactionAsync(Id);
    if (!succes) return NotFound();

    return Ok();
  }
}