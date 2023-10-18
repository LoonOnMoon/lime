using Lime.Domain.Entities;

namespace Lime.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserById(string id);

    User Add(User user);
}