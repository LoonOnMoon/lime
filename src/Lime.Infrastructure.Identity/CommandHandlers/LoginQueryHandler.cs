using Lime.Application.Authentication.Common;
using Lime.Application.Authentication.Queries.Login;
using Lime.Infrastructure.Identity.Models;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Lime.Infrastructure.Identity.CommandHandlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly UserManager<LimeUser> userManager;
    private readonly SignInManager<LimeUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public LoginQueryHandler(
        UserManager<LimeUser> userManager,
        SignInManager<LimeUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var signInResult = await this.signInManager.PasswordSignInAsync(query.Email, query.Password, false, true);

        // Get User
        var appUser = await this.userManager.FindByEmailAsync(query.Email);

        // Generate jwt token
        return new AuthenticationResult(
            JwtToken: "Sucessful login!");
    }
}