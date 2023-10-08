using System.Security.Claims;

using Lime.Application.Authentication.Queries.Login;
using Lime.Contracts.Authentication;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.Controllers;

[Authorize]
[Route("api/auth")]

// [ApiVersion("1.0")]
public class AuthenticationController : ApiController
{
    private readonly IMediator mediator;

    public AuthenticationController(
        IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var authResult = await this.mediator.Send(query);

        return this.Ok(new AuthenticationResponse(authResult.JwtToken));
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await Task.FromResult(0);

        // // Add User
        // var appUser = new LimeUser { UserName = model.Email, Email = model.Email };
        // var identityResult = await this.userManager.CreateAsync(appUser, model.Password);
        // if (!identityResult.Succeeded)
        // {
        //     return this.BadRequest();
        // }

        // Generate jwt token
        return this.Ok();
    }
}