using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Domain.Entities;

namespace RM.Domain.Interfaces.Repositories
{
    public interface IPurchaseRepository
    {
        Task AddAsync(List<Item> Items);
        Task<List<Item>> GetPurchases(string document);
    }
}