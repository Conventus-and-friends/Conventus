namespace Conventus.Server.Extensions;

public static class ConventusOpenIddictServiceExtensions
{
    public static IServiceCollection AddConventusOpenIddict(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("account/auth/token");

                options.AllowClientCredentialsFlow();

                options.AddDevelopmentSigningCertificate()
                       .AddDevelopmentEncryptionCertificate();

                options.UseAspNetCore().EnableTokenEndpointPassthrough();
            });
        return services;
    }
}
