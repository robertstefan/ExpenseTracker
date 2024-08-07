using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
  [ApiController]
  [Route("api/expenses/")]
  public class ExpensesController : ControllerBase
  {
    private readonly ExpenseService _expenseService;

    public ExpensesController(ExpenseService expenseService)
    {
      _expenseService = expenseService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<ExpenseDTO>> GetExpenses()
    {
      List<Expense> expenses = (await _expenseService.GetAllExpensesAsync()).ToList();

      if (expenses == null)
      {
        return Ok(Enumerable.Empty<ExpenseDTO>());
      }

      List<ExpenseDTO> result = new List<ExpenseDTO>();

      for (int i = 0; i < expenses.Count; i++)
      {
        Expense item = expenses[i];

        result.Add(new ExpenseDTO()
        {
          Id = item.Id,
          Amount = item.Amount,
          Category = item.Category,
          Date = item.Date,
          Description = item.Description
        });
      }

      return Ok(result);
    }

    [HttpGet]
    [Route(":id")]
    public async Task<ActionResult<ExpenseDTO>> GetExpense(Guid id)
    {
      var expense = await _expenseService.GetExpenseByIdAsync(id);

      if (expense == null)
      {
        return NotFound();
      }

      return new ExpenseDTO() { Id = expense.Id, Amount = expense.Amount, Category = expense.Category, Date = expense.Date, Description = expense.Description };
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateExpense(ExpenseDTO expenseDto)
    {
      var expense = new Expense()
      {
        Id = expenseDto.Id,
        Amount = expenseDto.Amount,
        Category = expenseDto.Category,
        Date = expenseDto.Date,
        Description = expenseDto.Description
      };

      await _expenseService.AddExpenseAsync(expense);

      return Created();
    }

    [HttpPut]
    public async Task<ActionResult<Expense>> UpdateExpense(Guid id, ExpenseDTO expenseDto)
    {
      if (id != expenseDto.Id)
      {
        return BadRequest();
      }

      var expense = new Expense()
      {
        Id = expenseDto.Id,
        Amount = expenseDto.Amount,
        Category = expenseDto.Category,
        Date = expenseDto.Date,
        Description = expenseDto.Description
      };

      await _expenseService.UpdateExpenseAsync(expense);

      return NoContent();
    }
  }
}
