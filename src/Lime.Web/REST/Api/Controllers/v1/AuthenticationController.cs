using Lime.Application.Authentication.Commands.Register;
using Lime.Application.Authentication.Queries.Login;
using Lime.Web.REST.Api.Models.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.REST.Api.Controllers;

[Authorize]
[Route("api/auth")]
[ApiVersion("1.0")]
public class AuthenticationController : ApiController
{
    private readonly ISender mediator;
    private readonly IMapper mapper;

    public AuthenticationController(
        ISender mediator,
        IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = this.mapper.Map<LoginQuery>(request);
        var authResult = await this.mediator.Send(query);

        return this.Ok(this.mapper.Map<AuthenticationResponse>(authResult));
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = this.mapper.Map<RegisterCommand>(request);
        var authResult = await this.mediator.Send(command);

        return this.Ok(this.mapper.Map<AuthenticationResponse>(authResult));
    }
}