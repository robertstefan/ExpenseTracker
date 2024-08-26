using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/raport")]
public class RaportController(RaportService _raportService) : ControllerBase
{
  [Route("all")]
  [HttpPost]
  public async Task<ActionResult<RaportDTO>> GetRaport(int? raportTimespan, DateTime? day, int? year, int? weekNumber, DateRange? raportPeriod)
  {
    // @TODO - take userid from CTX/CONTEXT
    var response = await _raportService.GetAllTransactionsHistory((RaportTimespanType)raportTimespan, 2, day, null, null, null);

    return Ok(response);
  }



}
