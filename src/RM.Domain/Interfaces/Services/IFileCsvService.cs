using System.Collections.Generic;
using RM.Domain.Services.Dtos;

namespace RM.Domain.Interfaces.Services
{
    public interface IFileCsvService
    {
         List<ProductDto> ReadFile(string file);
    }
}