using System.Threading.Tasks;
using RM.Domain.Entities;

namespace RM.Domain.Interfaces.Repositories
{
    public interface IPurchaseRepository
    {
         Task AddAsync(Purchase purchase);
    }
}