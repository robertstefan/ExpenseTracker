using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Transactions;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionsController(TransactionService _transactionService, ILogger<TransactionsController> _logger, ICurrencyExchangeProvider _exchangeProvider) : ControllerBase
{
  [HttpGet("list")]
  public async Task<ActionResult<List<Transaction>>> GetTransactionsPaginated([FromQuery] int offset = 0, [FromQuery] int limit = 10)
  {
    List<Transaction> transactions = (await _transactionService.GetTransactionsPaginatedAsync(offset, limit)).ToList();

    if (transactions == null)
    {
      return Ok(Enumerable.Empty<Transaction>());
    }

    return Ok(transactions.Select(transaction => new TransactionDTO(transaction)).ToList());
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
  {
    try
    {
      var transaction = await _transactionService.GetTransactionByIdAsync(id);

      if (transaction == null)
      {
        return NotFound();
      }

      return Ok(new TransactionDTO(transaction));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message);

      return BadRequest($"Could not retrieve the transaction with the id {id}");
    }
  }

  [HttpPost]
  public async Task<ActionResult<Transaction>> AddTransaction(CreateTransactionRequest transactionModel)
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
      return BadRequest($"Date cannot be lower than {DateTime.MinValue.Date.ToShortDateString()}");
    }

    TransactionType transactionTypeEnum;// = CastStringToTransactionType(transactionModel.TransactionType);
    Enum.TryParse(transactionModel.TransactionType, out transactionTypeEnum);
    try
    {
      double exchangeRate = _exchangeProvider[transactionModel.currency] != 0 ?
        _exchangeProvider[transactionModel.currency] : 1.0;

      Transaction transaction = Transaction.CreateNew(
        transactionModel.Description,
        transactionModel.Amount,
        transactionModel.Date,
        transactionModel.CategoryId,
        transactionModel.IsRecurrent,
        transactionTypeEnum,
        transactionModel.UserId,
        transactionModel.currency,
        exchangeRate
      );
      Guid result = await _transactionService.AddTransactionAsync(transaction);

      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message);
      return BadRequest("Could not register the transaction");
    }

  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Transaction>> UpdateTransaction(Guid id, UpdateTransactionRequest request)
  {
    if (id != request.Id)
    {
      return BadRequest();
    }

    try
    {
      TransactionType transactionTypeEnum = CastStringToTransactionType(request.TransactionType);

      var transaction = Transaction.Create(
        request.Id,
        request.Description,
        request.Amount,
        request.Date,
        request.CategoryId,
        request.IsRecurrent,
        transactionTypeEnum,
        request.UserId
      );

      var result = await _transactionService.UpdateTransactionAsync(transaction);

      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.Message);
      return BadRequest("Could not update the transaction");
    }
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Transaction>> DeleteTransaction(Guid id)
  {
    await _transactionService.DeleteTransactionAsync(id);

    return NoContent();
  }


  [HttpGet("categories/{categoryId}")]
  public async Task<ActionResult<List<Transaction>>> GetTransactionsByCategory(Guid categoryId)
  {
    if (categoryId == Guid.Empty)
    {
      return BadRequest("Category identifier not valid");
    }

    return Ok(await _transactionService.GetTransactionByCategoryIdAsync(categoryId));
  }

  private static TransactionType CastStringToTransactionType(string transactionType)
  {

    if (!Enum.TryParse(transactionType.Trim(), true, out TransactionType transactionTypeEnum))
    {
      transactionTypeEnum = TransactionType.Expense;
    }
    return transactionTypeEnum;
  }
}

