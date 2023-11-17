using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManager.Application.Geometry;

namespace UsersManager.WebApi.Controllers;

public class TestsController : BaseController
{
    [HttpGet]
    [Authorize("Adult")]
    public IActionResult AdultContentAsync() => Ok("The earth is flat.");

    [HttpGet]
    public Task<IActionResult> ThrowException() => throw new Exception();

    [HttpGet]
    public IActionResult GetTriangleAreaAsync([Required] float a, [Required] float b, [Required] float c)
    {
        if (Triangle.TryCreate(a, b, c, out var triangle))
            return Ok(triangle.Area);

        return BadRequest("Такой треугольник не возможен.");
    }
}
