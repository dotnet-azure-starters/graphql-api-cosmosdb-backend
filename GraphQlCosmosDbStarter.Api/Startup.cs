using GraphQlCosmosDbStarter.Api.Extensions;
using GraphQlCosmosDbStarter.Api.GraphQl;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace GraphQlCosmosDbStarter.Api
{
    /// <summary>
    ///     The main start-up class for the application.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">
        ///     The application configuration, where key value pair settings are stored (See
        ///     http://docs.asp.net/en/latest/fundamentals/configuration.html).
        /// </param>
        /// <param name="webHostEnvironment">
        ///     The environment the application is running under. This can be Development,
        ///     Staging or Production by default (See http://docs.asp.net/en/latest/fundamentals/environments.html).
        /// </param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     Configures the services to add to the ASP.NET Core Injection of Control (IoC) container. This method gets
        ///     called by the ASP.NET runtime (See
        ///     http://blogs.msdn.com/b/webdev/archive/2014/06/17/dependency-injection-in-asp-net-vnext.aspx).
        /// </summary>
        /// <param name="services">The services.</param>
        public virtual void ConfigureServices(IServiceCollection services) => services
            .AddApplicationInsightsTelemetry(
                _configuration) // Add Azure Application Insights data collection services to the services container.
            .AddCustomCaching()
            .AddCustomOptions(_configuration)
            .AddCustomRouting()
            .AddCustomHealthChecks(_webHostEnvironment, _configuration)
            .AddHttpContextAccessor()
            .AddControllers()
            .Services
            .AddCustomGraphQl(_webHostEnvironment, _configuration)
            .AddOptions()

            .AddProjectMappers()
            .AddProjectServices()
            .AddProjectRepositories()
            .AddCosmosDb(_configuration);

        /// <summary>
        ///     Configures the application and HTTP request pipeline. Configure is called after ConfigureServices is
        ///     called by the ASP.NET runtime.
        /// </summary>
        /// <param name="application">The application builder.</param>
        public virtual void Configure(IApplicationBuilder application)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }

            application
                .UseForwardedHeaders()
                .UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()
                 )
                .UseRouting()
                .UseEndpoints(
                    builder =>
                    {
                        var options = new GraphQLServerOptions { Tool = { Enable = false } };

                        // Map the GraphQL HTTP and web socket endpoint at /graphql.
                        builder.MapGraphQL().WithOptions(options);

                        builder.MapControllers();

                        // Map the GraphQL Playground UI to try out the GraphQL API at /.
                        builder.MapGraphQLPlayground("/");

                        // Map the GraphQL Voyager UI to let you navigate your GraphQL API as a spider graph at /voyager.
                        builder.MapGraphQLVoyager("/voyager");

                        // Map the GraphQL Banana Cake Pop UI to let you navigate your GraphQL API at /banana.
                        builder.MapBananaCakePop("/banana");

                        // Map health check endpoints.
                        builder.MapHealthChecks("/status");
                        builder.MapHealthChecks("/status/self", new HealthCheckOptions { Predicate = _ => false });

                        builder.MapHealthChecks("/health", new HealthCheckOptions
                        {
                            ResponseWriter = (context, report) => context.Response.WriteAsync("Ok")
                        });
                    });
        }
    }
}
