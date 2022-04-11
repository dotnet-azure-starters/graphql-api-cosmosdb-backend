using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace GraphQlCosmosDbStarter.Api.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods which extend ASP.NET Core services.
    /// </summary>
    public static class CustomServiceCollectionExtensions
    {
        /// <summary>
        /// Configures caching for the application. Registers the <see cref="IDistributedCache"/> types with the services collection or
        /// IoC container. The <see cref="IDistributedCache"/> is intended to be used in cloud hosted scenarios where there is a shared
        /// cache, which is shared between multiple instances of the application.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with caching services added.</returns>
        public static IServiceCollection AddCustomCaching(this IServiceCollection services) =>
            services
                .AddMemoryCache()
                .AddDistributedMemoryCache();

        /// <summary>
        /// Configures the settings by binding the contents of the appsettings.json file to the specified Plain Old CLR
        /// Objects (POCO) and adding <see cref="IOptions{T}"/> objects to the services collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The services with caching services added.</returns>
        public static IServiceCollection AddCustomOptions(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .Configure<ApplicationOptions>(configuration)
                .Configure<ForwardedHeadersOptions>(configuration.GetSection(nameof(ApplicationOptions.ForwardedHeaders)))
                .Configure<ForwardedHeadersOptions>(
                    options =>
                    {
                        options.KnownNetworks.Clear();
                        options.KnownProxies.Clear();
                    })
                .Configure<GraphQLOptions>(configuration.GetSection(nameof(ApplicationOptions.GraphQL)))
                .Configure<AppSettings>(configuration.GetSection(nameof(ApplicationOptions.AppSettings)).GetSection(nameof(ApplicationOptions.AppSettings)));

        /// <summary>
        /// Add custom routing settings which determines how URL's are generated.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with caching services added.</returns>
        public static IServiceCollection AddCustomRouting(this IServiceCollection services) =>
            services.AddRouting(options => options.LowercaseUrls = true);

        public static IServiceCollection AddCustomHealthChecks(
            this IServiceCollection services,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration) =>
            services
                .AddHealthChecks() // Add health checks for external dependencies here. See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
                .Services;
    }
}
