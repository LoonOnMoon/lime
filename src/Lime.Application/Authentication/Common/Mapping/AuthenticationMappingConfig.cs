namespace Lime.Application.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<(IdentityResult IdentityResult, User User), AuthenticationResult>()
        //     .Map(dest => dest.Organization, src => src.User.Organization)
        //     .Map(dest => dest, src => src.IdentityResult);
    }
}