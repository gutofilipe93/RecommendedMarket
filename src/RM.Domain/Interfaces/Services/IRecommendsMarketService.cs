using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Domain.Entities;
using RM.Domain.Services.Dtos;

namespace RM.Domain.Interfaces.Services
{
    public interface IRecommendsMarketService
    {
         Task<RecommendsMarket> GetRecommendsMarketService(List<string> itemsPurchase);
    }
}