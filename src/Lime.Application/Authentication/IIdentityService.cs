using Lime.Application.Authentication.Common;

namespace Lime.Application.Authentication;

public interface IIdentityService
{
    public Task<IdentityResult> Login(string userNameOrEmail, string password);

    public Task<IdentityResult> Register(string userName, string email, string password);
}