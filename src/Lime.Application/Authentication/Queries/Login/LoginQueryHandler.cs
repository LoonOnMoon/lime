using Lime.Application.Authentication.Common;
using Lime.Application.Authentication.Queries.Login;
using Lime.Application.Common.Interfaces.Persistence;
using Lime.Domain.Entities;

using MapsterMapper;

using MediatR;

namespace Lime.Application.Authentication.Commands.Register;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly ISender sender;
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public LoginQueryHandler(
        ISender sender,
        IMapper mapper,
        IUserRepository userRepository)
    {
        this.sender = sender;
        this.mapper = mapper;
        this.userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var query = this.mapper.Map<LoginIdentityQuery>(request);
        var identityResult = await this.sender.Send(query);

        var user = this.userRepository.GetUserById(identityResult.Id);

        if (user is null)
        {
            throw new Exception();
        }

        // return this.mapper.Map<AuthenticationResult>(identityResult);
        return new AuthenticationResult(
            Token: identityResult.Token,
            Organization: user.Organization,
            UserName: identityResult.UserName);
    }
}