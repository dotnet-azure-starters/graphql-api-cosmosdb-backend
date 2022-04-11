using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQlCosmosDbStarter.Data.Models;

namespace GraphQlCosmosDbStarter.Data
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<T>> GetMultipleAsync<T>(int offset, int limit) where T : BaseEntity;
        Task<T> GetAsync<T>(string id) where T : BaseEntity;
        Task AddAsync<T>(T itemToAdd) where T : BaseEntity;
        Task UpdateAsync<T>(string id, T itemToUpdate) where T : BaseEntity;
        Task DeleteAsync(string id);
    }
}
