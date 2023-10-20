using Lime.Domain.User;

namespace Lime.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(
    UserId UserId) : IRequest<User>;