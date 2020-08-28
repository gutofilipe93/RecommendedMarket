using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services.Dtos;
using RM.Domain.Services.Helpers;

namespace RM.Domain.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IFileCsvService _fileCsvService;
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(IFileCsvService fileCsvService, IPurchaseRepository purchaseRepository)
        {
            _fileCsvService = fileCsvService;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<ResponseApiHelper> AddPurchaseAsync(string file)
        {
            var productsDto = _fileCsvService.ReadFile(file);
            Purchase purchase = FormatPurchase(productsDto);
            await _purchaseRepository.AddAsync(purchase);
            return new ResponseApiHelper {Message =  "Compra cadastrada", Success = true, Data = purchase};
        }
        
        private  Purchase FormatPurchase(List<ProductDto> productsDto)
        {
            Purchase purchase = new Purchase
            {
                Market = productsDto.FirstOrDefault(x => x.Mercado != null)?.Mercado,
                PurchaseDate = productsDto.FirstOrDefault(x => x.DataCompra != null)?.DataCompra,
                Processed = false,
                Items = new List<Item>()
            };
            foreach (var product in productsDto)
            {
                Item item = FormatItem(product);
                purchase.Items.Add(item);
            }

            return purchase;
        }
        private  Item FormatItem(ProductDto product)
        {
            return new Item
            {
                Name = product.Nome,
                SearchableName = product.NomePesquisa,
                Price = product.Preco,
                HaveOffer = product.TemOferta == 1 ? true : false
            };
        }
    }
}