using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Domain.Entities;

namespace RM.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> GetAsync(string market);
        Task AddAsync(List<Product> products, string market);
        Task<ICollection<string>> GetSearchableNamesAsync();
        Task AddSearchableNamesAsync(List<string> names);
        Task<ICollection<string>> GetMarkets();
        Task DeleteAsync(string name);
    }
}