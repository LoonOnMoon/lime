using Lime.Application.Authentication;
using Lime.Application.Authentication.Commands.Errors;
using Lime.Infrastructure.Identity.Common;
using Lime.Infrastructure.Identity.Models;

using MapsterMapper;

using Microsoft.AspNetCore.Identity;

using IdentityResult = Lime.Application.Authentication.Common.IdentityResult;

namespace Lime.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<LimeIdentityUser> userManager;
    private readonly SignInManager<LimeIdentityUser> signInManager;
    private readonly IMapper mapper;
    private readonly JwtTokenGenerator jwtTokenGenerator;

    public IdentityService(
        UserManager<LimeIdentityUser> userManager,
        SignInManager<LimeIdentityUser> signInManager,
        IMapper mapper,
        JwtTokenGenerator jwtTokenGenerator)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.mapper = mapper;
        this.jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<IdentityResult> Login(string userNameOrEmail, string password)
    {
        var signInResult = await this.signInManager.PasswordSignInAsync(userNameOrEmail, password, false, true);

        // Get User
        var identityUser = await this.userManager.FindByNameAsync(userNameOrEmail);

        if (identityUser is null)
        {
            throw new Exception();
        }

        var token = this.jwtTokenGenerator.GenerateToken(identityUser);
        return this.mapper.Map<IdentityResult>((identityUser, token));
    }

    public async Task<IdentityResult> Register(string userName, string email, string password)
    {
        if (await this.userManager.FindByNameAsync(userName) is not null)
        {
            throw new DuplicateUserNameException();
        }

        if (await this.userManager.FindByEmailAsync(email) is not null)
        {
            throw new DuplicateEmailException();
        }

        var identityUser = new LimeIdentityUser()
        {
            UserName = userName,
            Email = email,
        };
        var identityResult = await this.userManager.CreateAsync(identityUser, password);

        var token = this.jwtTokenGenerator.GenerateToken(identityUser);
        return this.mapper.Map<IdentityResult>((identityUser, token));
    }
}