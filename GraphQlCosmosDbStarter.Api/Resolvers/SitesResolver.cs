using GraphQlCosmosDbStarter.Api.Responses;
using GraphQlCosmosDbStarter.Data;
using GraphQlCosmosDbStarter.Data.Models;
using Microsoft.Azure.Cosmos.Linq;

namespace GraphQlCosmosDbStarter.Api.Resolvers
{
    public class SitesResolver
    {
        private readonly ILogger<SitesResolver> _logger;
        private ICosmosDbService _cosmosDbService;

        public SitesResolver(ILogger<SitesResolver> logger, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        public async Task<PagedResponse<Site>> Sites(int? offset, int? limit)
        {
            var sites = await _cosmosDbService.GetMultipleAsync<Site>(offset ?? 0, limit ?? 50);
            return new PagedResponse<Site>
            {
                HasNextPage = true,
                HasPreviousPage = false,
                Items = sites,
                TotalCount = 200
            };
        }

        public async Task<Site> SiteBySiteNumber(string siteId) => await _cosmosDbService.GetAsync<Site>(siteId);
    }
}