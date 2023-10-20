namespace Lime.Domain.User.Entities;

public class AdminInvite
{
    public Guid InviteToken { get; set; }

    public required UserId CreatedBy { get; set; }

    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

    public DateTime ExpiresOn { get; init; } = DateTime.UtcNow.AddDays(1);
}