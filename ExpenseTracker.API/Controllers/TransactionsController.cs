using ExpenseTracker.API.Common.Extensions;
using ExpenseTracker.API.Common.Options;
using ExpenseTracker.API.Common.Pagination;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Common;
using ExpenseTracker.API.Requests.Transactions;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace ExpenseTracker.API.Controllers;

[Route("api/transactions")]
public class TransactionsController : ApiController
{
  private readonly TransactionService _transactionService;
  private readonly SoftDeleteSettings _softDeleteSettings;
  public TransactionsController(TransactionService transactionService, IOptions<SoftDeleteSettings> softDeleteSettings)
  {
    _transactionService = transactionService;
    _softDeleteSettings = softDeleteSettings.Value;
  }
  [HttpGet("list")]
  public async Task<ActionResult<Paged<TransactionDTO>>> GetTransactionsPaginated([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 10)
  {
    var transactions = await _transactionService.GetTransactionsPaginatedAsync(PageNumber, PageSize);

    if (transactions == null)
    {
      return Ok(Enumerable.Empty<Transaction>());
    }

    return new Paged<TransactionDTO>(transactions.TotalCount, PageNumber, PageSize)
    {
      Items = transactions.Rows.Select(t => new TransactionDTO(t)).ToList(),
      TotalItems = transactions.TotalCount,
    };

  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
  {
    if (id.IsEmpty())
    {
      return BadRequest("The id could not be null");
    }
    try
    {
      var transaction = await _transactionService.GetTransactionByIdAsync(id);

      if (transaction == null)
      {
        Log.Error("Could not retrieve the transaction with the id {id}", id);
        return NotFound("No transactions with the associated id were found.");
      }

      return Ok(new TransactionDTO(transaction));
    }
    catch (Exception ex)
    {
      Log.Fatal("Could not retrieve the transaction with the id {id}", id, ex);

      return BadRequest("The transaction could not be retrieved");
    }
  }

  [HttpPost]
  public async Task<ActionResult<Transaction>> AddTransaction(CreateTransactionRequest transactionModel)
  {
    var validationErrors = transactionModel.GetValidationErrors();

    if (validationErrors.Count != 0)
    {
      return BadRequest(new { Errors = validationErrors });
    }

    try
    {
      TransactionType transactionTypeEnum = IntToEnum.Handle<TransactionType>(transactionModel.TransactionType)!.Value;

      var transaction = Transaction.CreateNew(
        transactionModel.Description,
        transactionModel.Amount,
        transactionModel.Date,
        transactionModel.CategoryId,
        transactionModel.SubcategoryId,
        transactionModel.IsRecurrent,
        transactionTypeEnum
      );
      Guid result = await _transactionService.AddTransactionAsync(transaction);

      if (result == Guid.Empty)
      {
        Log.Error("The transaction could not be created.");
        return NoContent();
      }

      Log.Information("The transaction was created.");

      return Ok(result);
    }
    catch (Exception ex)
    {
      Log.Fatal(ex, "The transaction could not be created.");
      return BadRequest("Could not register the transaction");
    }

  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Transaction>> UpdateTransaction(Guid id, UpdateTransactionRequest request)
  {
    if (id.IsEmpty())
    {
      return BadRequest("The id could not be null");
    }
    var validationErrors = request.GetValidationErrors();

    if (validationErrors.Count != 0)
    {
      return BadRequest(new { Errors = validationErrors });
    }
    try
    {
      TransactionType transactionTypeEnum = IntToEnum.Handle<TransactionType>(request.TransactionType)!.Value;
      var transaction = Transaction.Create(
        id,
        request.Description,
        request.Amount,
        request.Date,
        request.CategoryId,
        request.SubcategoryId,
        request.IsRecurrent,
        transactionTypeEnum
      );

      var updateSuccess = await _transactionService.UpdateTransactionAsync(transaction);

      if (updateSuccess)
      {
        Log.Information("The transaction was updated.");
        return Ok();
      }

      Log.Error("The transaction with the id {id} could not be updated");

      return NoContent();
    }
    catch (Exception ex)
    {
      Log.Fatal("The transaction with the id {id} could not be updated", id, ex);
      return BadRequest("Could not update the transaction");
    }
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Transaction>> DeleteTransaction(Guid id)
  {
    if (id.IsEmpty())
    {
      return BadRequest("The id could not be empty.");
    }
    try
    {
      var softDelete = _softDeleteSettings.SoftDelete;
      var deleteSuccess = await _transactionService.DeleteTransactionAsync(id, softDelete);

      if (deleteSuccess)
      {
        Log.Information("The transaction with the id {id} was deleted", id);

        return Ok();
      }
      Log.Error("The transaction with the id {id} could not be deleted", id);

      return NoContent();
    }
    catch (Exception ex)
    {
      Log.Fatal("The transaction with the id {id} could not be deleted", id, ex);
      return BadRequest("The transaction could not be deleted");
    }
  }
}

