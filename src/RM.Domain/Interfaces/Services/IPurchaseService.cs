using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Domain.Entities;
using RM.Domain.Services.Dtos;
using RM.Domain.Services.Helpers;

namespace RM.Domain.Interfaces.Services
{
    public interface IPurchaseService
    {
        Task<ResponseApiHelper> AddPurchaseAsync(string file);
        Task<ResponseApiHelper> AddPurchaseAsync(List<ProductDto> productsDto);
        Task<Dictionary<string,decimal>> GetAllAsync();

    }
}