using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[EnableCors("WebPolicy")]
// [Authorize]
public class ApiController : ControllerBase
{

}
