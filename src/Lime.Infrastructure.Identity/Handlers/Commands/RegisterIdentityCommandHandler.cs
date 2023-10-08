using Lime.Application.Authentication.Commands.Errors;
using Lime.Application.Authentication.Commands.Register;
using Lime.Application.Authentication.Common;
using Lime.Infrastructure.Identity.Common;
using Lime.Infrastructure.Identity.Models;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Identity;

using IdentityResult = Lime.Application.Authentication.Common.IdentityResult;

namespace Lime.Infrastructure.Identity.Handlers.Queries;

public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, IdentityResult>
{
    private readonly UserManager<LimeIdentityUser> userManager;
    private readonly IMapper mapper;
    private readonly JwtTokenGenerator jwtTokenGenerator;

    public RegisterIdentityCommandHandler(
        UserManager<LimeIdentityUser> userManager,
        IMapper mapper,
        JwtTokenGenerator jwtTokenGenerator)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<IdentityResult> Handle(RegisterIdentityCommand command, CancellationToken cancellationToken)
    {
        if (await this.userManager.FindByNameAsync(command.UserName) is not null)
        {
            throw new DuplicateUserNameException();
        }

        if (await this.userManager.FindByEmailAsync(command.Email) is not null)
        {
            throw new DuplicateEmailException();
        }

        var identityUser = this.mapper.Map<LimeIdentityUser>(command);
        var identityResult = await this.userManager.CreateAsync(identityUser, command.Password);

        var token = this.jwtTokenGenerator.GenerateToken(identityUser);
        return this.mapper.Map<IdentityResult>((identityUser, token));
    }
}