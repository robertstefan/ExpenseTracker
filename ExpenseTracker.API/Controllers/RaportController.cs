using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Route("api/raport")]
public class RaportController(RaportService _raportService) : ApiController
{
    [Route("summary")]
    [HttpPost]
    public async Task<ActionResult<RaportDTO>> GetRaport(Guid userId, int? raportTimespan, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod)
    {
        // var _userId = HttpContext.User.Claims.FirstOrDefault(uId => uId.).ToString();
        var response = await _raportService.GetAllTransactionsHistory
        (
            raportTimespanType: (RaportTimespanType)raportTimespan,
            userId: userId,
            day: day,
            month: month,
            year: year,
            weekNumber: weekNumber,
            raportPeriod: raportPeriod
        );

        return Ok(response);
    }

    [Route("categories")]
    [HttpPost]
    public async Task<ActionResult<RaportDTO>> TopCategories(Guid userId, int? raportTimespan, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod)
    {
        // @TODO - take userid from CTX/CONTEXT

        var response = await _raportService.TopCategories(
            raportTimespanType: (RaportTimespanType)raportTimespan,
            userId: userId,
            day: day,
            month: month,
            year: year,
            weekNumber: weekNumber,
            raportPeriod: raportPeriod);

        Guid id = Guid.NewGuid();

        return Ok(new { Id = id, Items = response.Select(c => new { c.Id, c.Name, c.CategoryIncome, c.CategoryOutcome }).ToList() });
    }
}
