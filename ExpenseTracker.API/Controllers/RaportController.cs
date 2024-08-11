using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/raport")]
public class RaportController(RaportService _raportService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RaportDTO>> GetRaport([FromQuery] string raportTimespan = "Day", [FromQuery] string raportType = "Total")
    {

        var response = await _raportService.GetRaportAsync(raportTimespan);

        return Ok();
    }



}
