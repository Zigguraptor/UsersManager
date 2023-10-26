using Microsoft.AspNetCore.Mvc;

namespace UsersManager.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
}
