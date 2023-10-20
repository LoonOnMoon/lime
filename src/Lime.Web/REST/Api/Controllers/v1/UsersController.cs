using Lime.Application.Authentication.Commands.Register;
using Lime.Application.Authentication.Queries.Login;
using Lime.Web.REST.Api.Models.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.REST.Api.Controllers;

[Authorize]
[Route("api/users")]
[ApiVersion("1.0")]
public class UsersController : ApiController
{
    private readonly ISender mediator;
    private readonly IMapper mapper;

    public UsersController(
        ISender mediator,
        IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("create-admin-invite")]
    public async Task<IActionResult> CreateAdminInvite([FromBody] CreateAdminInviteRequest request)
    {
        var query = this.mapper.Map<LoginQuery>(request);
        var adminInviteResult = await this.mediator.Send(query);

        return this.Ok(this.mapper.Map<InviteResponse>(adminInviteResult));
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("create-admin")]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminInviteRequest request)
    {
        var command = this.mapper.Map<RegisterCommand>(request);
        var authResult = await this.mediator.Send(command);

        return this.Ok(this.mapper.Map<InviteResponse>(authResult));
    }
}