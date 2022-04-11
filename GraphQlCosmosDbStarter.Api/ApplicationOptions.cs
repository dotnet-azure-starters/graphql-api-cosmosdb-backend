using System.ComponentModel.DataAnnotations;
using HotChocolate.Execution.Options;
using HotChocolate.Types.Pagination;

namespace GraphQlCosmosDbStarter.Api
{
    /// <summary>
    /// Options for the application.
    /// </summary>
    public class ApplicationOptions
    {
        public ForwardedHeadersOptions ForwardedHeaders { get; set; } = default!;
        
        public GraphQLOptions GraphQL { get; set; } = default!;

        public AppSettings AppSettings { get; set; } = default!;
    }

    public class GraphQLOptions
    {
        [Required]
        public int MaxAllowedExecutionDepth { get; set; }

        [Required]
        public PagingOptions Paging { get; set; } = default!;

        [Required]
        public RequestExecutorOptions Request { get; set; } = default!;
    }

    public class AppSettings
    {
        public string CosmosDbConnectionString { get; set; }
        public string CosmosDatabaseName { get; set; }
    }
}
