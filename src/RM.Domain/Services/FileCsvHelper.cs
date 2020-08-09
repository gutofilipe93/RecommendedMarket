using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services.Dtos;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace RM.Domain.Services
{
    public class FileCsvService : IFileCsvService
    {
        public List<ProductDto> ReadFile(string file)
        {
            List<CsvMappingResult<ProductDto>> result = new List<CsvMappingResult<ProductDto>>();
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');
            CsvUserDetailsMapping csvMapper = new CsvUserDetailsMapping();
            CsvParser<ProductDto> csvParser = new CsvParser<ProductDto>(csvParserOptions, csvMapper);
            result = csvParser
                         .ReadFromFile(file, Encoding.UTF8)
                         .ToList();

            return FormatCsvToProduct(result);
        }

        private List<ProductDto> FormatCsvToProduct(List<CsvMappingResult<ProductDto>> results)
        {
            List<ProductDto> products = new List<ProductDto>();
            foreach (var item in results)            
                products.Add(item.Result);
            
            return products;
        }
    }

    public class CsvUserDetailsMapping : CsvMapping<ProductDto>
    {
        public CsvUserDetailsMapping()
            : base()
        {
            MapProperty(0, x => x.Nome);
            MapProperty(1, x => x.Preco);
            MapProperty(2, x => x.TemOferta);
            MapProperty(3, x => x.NomePesquisa);
            MapProperty(4, x => x.Mercado);
            MapProperty(5, x => x.DataCompra);
        }
    } 
}