using Lime.Application.Authentication.Common;
using Lime.Application.Authentication.Queries.Login;
using Lime.Infrastructure.Identity.Common;
using Lime.Infrastructure.Identity.Models;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Identity;

using IdentityResult = Lime.Application.Authentication.Common.IdentityResult;

namespace Lime.Infrastructure.Identity.Handlers.Queries;

public class LoginIdentityQueryHandler : IRequestHandler<LoginIdentityQuery, IdentityResult>
{
    private readonly UserManager<LimeIdentityUser> userManager;
    private readonly SignInManager<LimeIdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IMapper mapper;
    private readonly JwtTokenGenerator jwtTokenGenerator;

    public LoginIdentityQueryHandler(
        UserManager<LimeIdentityUser> userManager,
        SignInManager<LimeIdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        JwtTokenGenerator jwtTokenGenerator)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.mapper = mapper;
        this.jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<IdentityResult> Handle(LoginIdentityQuery query, CancellationToken cancellationToken)
    {
        var signInResult = await this.signInManager.PasswordSignInAsync(query.UserNameOrEmail, query.Password, false, true);

        // Get User
        var identityUser = await this.userManager.FindByNameAsync(query.UserNameOrEmail);

        if (identityUser is null)
        {
            throw new Exception();
        }

        var token = this.jwtTokenGenerator.GenerateToken(identityUser);
        return this.mapper.Map<IdentityResult>((identityUser, token));
    }
}