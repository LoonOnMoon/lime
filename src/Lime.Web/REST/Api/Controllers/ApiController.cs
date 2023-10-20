using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.REST.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
}