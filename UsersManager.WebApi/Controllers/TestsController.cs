using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersManager.WebApi.Controllers;

public class TestsController : BaseController
{
    [HttpGet]
    [Authorize("Adult")]
    public IActionResult AdultContentAsync() => Ok("The earth is flat.");

    [HttpGet]
    public Task<IActionResult> ThrowException() => throw new Exception();
}
