using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.Controllers;

[ApiController]

// [Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
}