using Lime.Application.Authentication.Common;

namespace Lime.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IMapper mapper;
    private readonly IIdentityService identityService;

    public RegisterCommandHandler(
        IMapper mapper,
        IIdentityService identityService)
    {
        this.mapper = mapper;
        this.identityService = identityService;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var identityResult = await this.identityService.Register(request.UserName, request.Email, request.Password);

        // var user = this.userRepository.Add(new User()
        // {
        //     Id = identityResult.Id,
        //     Organization = request.Organization,
        // });

        return this.mapper.Map<AuthenticationResult>((identityResult, identityResult));
    }
}