using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersManager.WebApi.Controllers;

public class ContentController : BaseController
{
    [HttpGet]
    [Authorize("Adult")]
    public IActionResult AdultContentAsync() => Ok("The earth is flat.");
}
