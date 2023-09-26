using System.Security.Claims;

using Lime.Contracts.Authentication;
using Lime.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lime.Web.Controllers;

[Authorize]

// [ApiVersion("1.0")]
public class AuthenticationController : ApiController
{
    private readonly UserManager<LimeUser> userManager;
    private readonly SignInManager<LimeUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AuthenticationController(
        UserManager<LimeUser> userManager,
        SignInManager<LimeUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var signInResult = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
        if (!signInResult.Succeeded)
        {
            return this.BadRequest();
        }

        // Get User
        var appUser = await this.userManager.FindByEmailAsync(model.Email);

        // Generate jwt token
        return this.Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        // Add User
        var appUser = new LimeUser { UserName = model.Email, Email = model.Email };
        var identityResult = await this.userManager.CreateAsync(appUser, model.Password);
        if (!identityResult.Succeeded)
        {
            return this.BadRequest();
        }

        // Add UserRoles
        identityResult = await this.userManager.AddToRoleAsync(appUser, "Admin");
        if (!identityResult.Succeeded)
        {
            return this.BadRequest();
        }

        // Add UserClaims
        var userClaims = new List<Claim>
        {
            new Claim("Editors_Write", "Write"),
            new Claim("Editors_Remove", "Remove"),
        };
        await this.userManager.AddClaimsAsync(appUser, userClaims);

        // Generate jwt token
        return this.Ok();
    }
}