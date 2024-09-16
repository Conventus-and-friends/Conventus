namespace Conventus.Server.Extensions;

public static class ConventusOpenIddictServiceExtensions
{
    public static IServiceCollection AddConventusOpenIddict(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                // store oidc data in the database
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();

                // enable quartz
                options.UseQuartz();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("identity/authorization/token");

                options.AllowClientCredentialsFlow();

                options.AddDevelopmentSigningCertificate()
                       .AddDevelopmentEncryptionCertificate();

                options.UseAspNetCore().EnableTokenEndpointPassthrough();
            });
        return services;
    }
}
