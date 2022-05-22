using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Domain.Services.Dtos;
using RM.Domain.Services.Helpers;

namespace RM.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<ResponseApiHelper> AddProductsAndSearchableNamesAsync(string file);
        Task<ResponseApiHelper> AddProductsAndSearchableNamesListAsync(List<ProductDto> productsFile);
        Task<ICollection<string>> GetSearchableNamesAsync();
        Task<ResponseApiHelper> AdjustDuplicateNames(List<DuplicateName> duplicateNames); 
    }
}