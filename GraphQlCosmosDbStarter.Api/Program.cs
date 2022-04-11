using System.Reflection;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

namespace GraphQlCosmosDbStarter.Api
{
    public sealed class Program
    {
        public static async Task<int> Main(string[] args)
        {
            IHostEnvironment? hostEnvironment = null;

            try
            {
                Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
                Log.Information("Starting up....");

                var host = CreateHostBuilder(args).Build();
                hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
                hostEnvironment.ApplicationName = Assembly.GetExecutingAssembly().FullName;

                await host.RunAsync().ConfigureAwait(false);

                Log.Information("Stopped cleanly");
                return 0;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{Assembly.GetExecutingAssembly().FullName} terminated unexpectedly in {hostEnvironment?.EnvironmentName} mode.");
                Console.WriteLine(exception.ToString());

                Log.Fatal(exception, "An unhandled exception occurred during bootstrapping");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(configurationBuilder => configurationBuilder.AddEnvironmentVariables(prefix: "DOTNET_"))
                .ConfigureAppConfiguration((hostingContext, config) => AddConfiguration(config, hostingContext.HostingEnvironment, args))
                .UseDefaultServiceProvider(
                    (context, options) =>
                    {
                        var isDevelopment = context.HostingEnvironment.IsDevelopment();
                        options.ValidateScopes = isDevelopment;
                        options.ValidateOnBuild = isDevelopment;
                    })
                .UseSerilog(ConfigureLogger)
                .ConfigureWebHost(ConfigureWebHostBuilder)
                .UseConsoleLifetime();

        private static void ConfigureLogger(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithAssemblyInformationalVersion()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces);

        private static void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) =>
            webHostBuilder
                .UseKestrel((_, options) => options.AddServerHeader = false)
                .UseIIS() // Used for IIS and IIS Express for in-process hosting. Use UseIISIntegration for out-of-process hosting.
                .UseStartup<Startup>();

        private static IConfigurationBuilder AddConfiguration(
            IConfigurationBuilder configurationBuilder,
            IHostEnvironment hostEnvironment,
            string[] args)
        {
            configurationBuilder
                // Add configuration from the appsettings.json file.
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)

                // Add configuration specific to the Development, Staging or Production environments. This config can
                // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
                // override the ones in all of the above config files. See
                // http://docs.asp.net/en/latest/security/app-secrets.html
                .AddEnvironmentVariables()

                // Push telemetry data through the Azure Application Insights pipeline faster in the development and
                // staging environments, allowing you to view results immediately.
                .AddApplicationInsightsSettings(developerMode: !hostEnvironment.IsProduction());

            // Add command line options. These take the highest priority.
            if (args is not null)
            {
                configurationBuilder.AddCommandLine(args);
            }

            return configurationBuilder;
        }
    }
}
