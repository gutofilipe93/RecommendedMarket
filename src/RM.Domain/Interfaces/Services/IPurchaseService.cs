using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Domain.Services.Dtos;
using RM.Domain.Services.Helpers;

namespace RM.Domain.Interfaces.Services
{
    public interface IPurchaseService
    {
        Task<ResponseApiHelper> AddPurchaseAsync(string file);
        Task<ResponseApiHelper> AddPurchaseAsync(List<ProductDto> productsDto);
    }
}