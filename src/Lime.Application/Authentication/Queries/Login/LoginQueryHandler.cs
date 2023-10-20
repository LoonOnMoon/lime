using Lime.Application.Authentication.Common;
using Lime.Application.Authentication.Queries.Login;

using MapsterMapper;

using MediatR;

namespace Lime.Application.Authentication.Commands.Register;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IMapper mapper;
    private readonly IIdentityService identityService;

    public LoginQueryHandler(
        IMapper mapper,
        IIdentityService identityService)
    {
        this.mapper = mapper;
        this.identityService = identityService;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var identityResult = await this.identityService.Login(request.UserNameOrEmail, request.Password);

        throw new NotImplementedException();

        // var user = this.userRepository.GetUserById(identityResult.Id);

        // if (user is null)
        // {
        //     throw new Exception();
        // }

        // return this.mapper.Map<AuthenticationResult>((identityResult, user));
    }
}