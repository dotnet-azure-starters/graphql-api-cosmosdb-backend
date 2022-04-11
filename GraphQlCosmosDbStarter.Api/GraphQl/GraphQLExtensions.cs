using System.Reflection;
using System.Text;
using GraphQlCosmosDbStarter.Api.GraphQl.Directives;
using GraphQlCosmosDbStarter.Api.GraphQl.Filters;
using GraphQlCosmosDbStarter.Api.Resolvers;
using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Options;
using Microsoft.ApplicationInsights;

namespace GraphQlCosmosDbStarter.Api.GraphQl
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods which extend ASP.NET Core services.
    /// </summary>
    internal static class GraphQLExtensions
    {
        public static IServiceCollection AddCustomGraphQl(
            this IServiceCollection services,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            var graphQLOptions = configuration.GetSection(nameof(ApplicationOptions.GraphQL)).Get<GraphQLOptions>();
            return services
                .AddGraphQLServer()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = !webHostEnvironment.IsProduction())

                .AddCustomGraphQlSchema()
                .AddCustomResolvers()
                .AddCustomTypesAndFiltersAndListeners()
                .AddCustomDirectives()
                .AddScalarTypes()

                .AddApolloTracing(TracingPreference.OnDemand)
                .AddAuthorization()
                .TrimTypes()
                .ModifyOptions(options => options.UseXmlDocumentation = true)
                .AddMaxExecutionDepthRule(graphQLOptions.MaxAllowedExecutionDepth)
                .SetPagingOptions(graphQLOptions.Paging)
                .SetRequestOptions(() => graphQLOptions.Request)
                .Services;
        }

        public static IRequestExecutorBuilder AddCustomGraphQlSchema(this IRequestExecutorBuilder services)
        {
            var schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphQlCosmosDbStarter.Api.GraphQl.Schema.graphql");
            
            using var streamReader = new StreamReader(schemaStream, Encoding.Default);
            var schemaString = streamReader.ReadToEnd();

            services.AddDocumentFromString(schemaString);

            return services;
        }

        public static IRequestExecutorBuilder AddCustomResolvers(this IRequestExecutorBuilder services) =>
            services
                .AddResolver<SitesResolver>("Query");

        private static IRequestExecutorBuilder AddCustomTypesAndFiltersAndListeners(this IRequestExecutorBuilder services) =>
            services
                .AddErrorFilter<ErrorHandlerFilter>()
                .AddDiagnosticEventListener<AppInsightsDiagnosticEventListener>((sp) => new AppInsightsDiagnosticEventListener(sp.GetService<TelemetryClient>()));

        public static IRequestExecutorBuilder AddCustomDirectives(this IRequestExecutorBuilder builder) =>
            builder
                .AddDirectiveType<UpperCaseDirectiveType>()
                .AddDirectiveType<LowerCaseDirectiveType>()
                .AddDirectiveType<TitleCaseDirectiveType>();

        public static IRequestExecutorBuilder AddScalarTypes(this IRequestExecutorBuilder builder) =>
            builder
                .BindRuntimeType<DateTime, DateType>();
    }
}
