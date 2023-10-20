using Lime.Application.Users.Common;
using Lime.Application.Users.Queries;
using Lime.Domain.User;

namespace Lime.Application.Users.Commands.CreateAdminInvite;

public class CreateAdminInviteCommandHandler : IRequestHandler<CreateAdminInviteCommand, InviteResult>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IUserReadRepository userReadRepository;

    public CreateAdminInviteCommandHandler(
        IMapper mapper,
        IMediator mediator,
        IUserReadRepository userReadRepository)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.userReadRepository = userReadRepository;
    }

    public async Task<InviteResult> Handle(CreateAdminInviteCommand request, CancellationToken cancellationToken)
    {
        User user = this.userReadRepository.GetById(this.mapper.Map<UserId>(request.CreatedBy));

        var createAdminInviteResult = user.CreateAdminInvite();

        await Task.WhenAll(createAdminInviteResult.DomainEvents.Select(
            async domainEvent =>
            {
                await this.mediator.Publish(domainEvent);
            }));

        return this.mapper.Map<InviteResult>(createAdminInviteResult.Result);
    }
}