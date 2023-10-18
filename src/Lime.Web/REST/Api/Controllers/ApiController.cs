using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.REST.Api.Controllers;

// [ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
}