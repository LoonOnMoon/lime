using Lime.Application.Authentication.Commands.Register;
using Lime.Application.Authentication.Common;
using Lime.Domain.Entities;

using Mapster;

namespace Lime.Application.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(IdentityResult IdentityResult, User User), AuthenticationResult>()
            .Map(dest => dest.Organization, src => src.User.Organization)
            .Map(dest => dest, src => src.IdentityResult);
    }
}