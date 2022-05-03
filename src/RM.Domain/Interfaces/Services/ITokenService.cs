using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RM.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<dynamic> GetTokenAsync(string user, string password);
    }
}
