using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using RM.Domain.Entities;

namespace RM.Domain.Interfaces.Repositories
{
    public interface IPurchaseRepository
    {
        Task AddAsync(List<Item> Items);
        Task AddAsync(List<Item> Items, string key);
        Task<List<Item>> GetPurchasesAsync(string document);
        CollectionReference GetAll();
    }
}