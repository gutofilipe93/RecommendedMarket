using System.Threading.Tasks;
using RM.Domain.Entities;

namespace RM.Domain.Interfaces.Repositories
{
    public interface IRecommendsMarketRepository
    {
         Task SaveAsync(RecommendsMarket recommendsMarket);
         Task DeleteAsync();
         Task<RecommendsMarket> GetRecommendsMarket();
    }
}