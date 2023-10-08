using Lime.Application.Common.Interfaces.Persistence;
using Lime.Domain.Entities;

namespace Lime.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private static readonly List<User> Users = new();

    public User Add(User user)
    {
        Users.Add(user);

        return user;
    }

    public User? GetUserById(string id)
    {
        return Users.SingleOrDefault(u => u.Id == id);
    }
}