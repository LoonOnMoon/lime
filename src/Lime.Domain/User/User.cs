using Lime.Domain.Common;
using Lime.Domain.User.DomainEvents;
using Lime.Domain.User.Entities;

namespace Lime.Domain.User;

public class User
{
    public required UserId Id { get; set; }

    public ResultWithDomainEvents<AdminInvite, UserDomainEvent> CreateAdminInvite()
    {
        var adminInvite = new AdminInvite()
        {
            CreatedBy = this.Id,
            InviteToken = Guid.NewGuid(),
        };

        var result = new ResultWithDomainEvents<AdminInvite, UserDomainEvent>()
        {
            Result = adminInvite,
        };

        result.DomainEvents.Add(new AdminInviteCreatedDomainEvent()
        {
            AdminInvite = adminInvite,
            RaisedBy = this.Id,
        });

        return result;
    }
}

public class UserId
{
    public Guid Value { get; set; }
}