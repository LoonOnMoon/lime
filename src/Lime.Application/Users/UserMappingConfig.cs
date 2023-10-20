using Lime.Domain.User;

namespace Lime.Infrastructure.Identity.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, UserId>()
            .Map(dest => dest.Value, src => src);
    }
}