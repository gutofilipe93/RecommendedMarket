using System.Threading.Tasks;
using RM.Domain.Services.Helpers;

namespace RM.Domain.Interfaces.Services
{
    public interface IProductService
    {
         Task<ResponseApiHelper> AddProductsAysnc(string file);
    }
}