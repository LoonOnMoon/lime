using Lime.Application.Authentication.Common;
using Lime.Application.Common.Interfaces.Persistence;
using Lime.Domain.Entities;

using MapsterMapper;

using MediatR;

namespace Lime.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly ISender sender;
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public RegisterCommandHandler(
        ISender sender,
        IMapper mapper,
        IUserRepository userRepository)
    {
        this.sender = sender;
        this.mapper = mapper;
        this.userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<RegisterIdentityCommand>(request);
        var identityResult = await this.sender.Send(command);

        var user = this.userRepository.Add(new User()
        {
            Id = identityResult.Id,
            Organization = request.Organization,
        });

        // return this.mapper.Map<AuthenticationResult>(identityResult);
        return new AuthenticationResult(
            Token: identityResult.Token,
            Organization: user.Organization,
            UserName: identityResult.UserName);
    }
}