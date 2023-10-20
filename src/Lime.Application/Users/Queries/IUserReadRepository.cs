using Lime.Domain.User;

namespace Lime.Application.Users.Queries;

public interface IUserReadRepository
{
    public User GetById(UserId id);
}