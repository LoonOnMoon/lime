using Lime.Application.Authentication.Common;
using Lime.Application.Common.Interfaces.Persistence;
using Lime.Domain.Entities;

using MapsterMapper;

using MediatR;

namespace Lime.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;
    private readonly IIdentityService identityService;

    public RegisterCommandHandler(
        IMapper mapper,
        IUserRepository userRepository,
        IIdentityService identityService)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.identityService = identityService;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var identityResult = await this.identityService.Register(request.UserName, request.Email, request.Password);

        var user = this.userRepository.Add(new User()
        {
            Id = identityResult.Id,
            Organization = request.Organization,
        });

        return this.mapper.Map<AuthenticationResult>((identityResult, user));
    }
}