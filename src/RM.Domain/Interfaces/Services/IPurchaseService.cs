using System.Threading.Tasks;
using RM.Domain.Services.Helpers;

namespace RM.Domain.Interfaces.Services
{
    public interface IPurchaseService
    {
         Task<ResponseApiHelper> AddPurchaseAsync(string file);
    }
}