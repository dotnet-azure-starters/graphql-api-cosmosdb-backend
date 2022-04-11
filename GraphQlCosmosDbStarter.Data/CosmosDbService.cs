using GraphQlCosmosDbStarter.Data.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace GraphQlCosmosDbStarter.Data
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<T>> GetMultipleAsync<T>(int offset, int limit) where T : BaseEntity
        {
            return _container.GetItemLinqQueryable<T>(true).Skip(offset).Take(limit).ToList();
        }

        public async Task<T> GetAsync<T>(string id) where T : BaseEntity
        {
            try
            {
                var response = this._container.GetItemLinqQueryable<T>().Where(s => s.Id == id).FirstOrDefault();
                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public Task AddAsync<T>(T itemToAdd) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<T>(string id, T itemToUpdate) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
