using Lime.Application.Authentication.Commands.Register;
using Lime.Application.Authentication.Common;
using Lime.Infrastructure.Identity.Models;

using Mapster;

using Microsoft.Extensions.DependencyInjection;

namespace Lime.Infrastructure.Identity.Common.Mapping;

public class IdentityMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(LimeIdentityUser User, string Token), IdentityResult>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest, src => src.User);
    }
}