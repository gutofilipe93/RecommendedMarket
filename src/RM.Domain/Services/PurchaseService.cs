using System;
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

        public async Task<ResponseApiHelper> AddPurchaseAsync(List<ProductDto> productsDto)
        {
            var purchases = FormatPurchase(productsDto);
            var month = DateTime.Today.Month.ToString().Length == 1 ? $"0{DateTime.Today.Month}" : DateTime.Today.Month.ToString();
            var key = $"{month}-{DateTime.Today.Year}";
            var purchasesFirebase = await _purchaseRepository.GetPurchasesAsync(key);
            purchasesFirebase = purchasesFirebase.Union(purchases).ToList();
            await _purchaseRepository.AddAsync(purchasesFirebase);
            return new ResponseApiHelper { Message = "Compra cadastrada", Success = true, Data = purchases };
        }

        public async Task<ResponseApiHelper> AddPurchaseAsync(string file)
        {
            var productsDto = _fileCsvService.ReadFile(file);
            var purchases = FormatPurchase(productsDto);
            await _purchaseRepository.AddAsync(purchases);
            return new ResponseApiHelper {Message =  "Compra cadastrada", Success = true, Data = purchases};
        }
        
        private  List<Item> FormatPurchase(List<ProductDto> productsDto)
        {
            var items = new List<Item>();
            foreach (var product in productsDto)
            {
                Item item = FormatItem(product);
                items.Add(item);
            }

            return items;
        }
        private  Item FormatItem(ProductDto product)
        {
            return new Item
            {
                Name = product.Nome,
                SearchableName = product.NomePesquisa,
                Price = product.PrecoTotal,
                HaveOffer = product.TemOferta == 1 ? true : false
            };
        }

        public async Task<Dictionary<string, decimal>> GetAllAsync()
        {
            var purchasesFirebase =  _purchaseRepository.GetAll();
            var documents = await purchasesFirebase.GetSnapshotAsync();
            var orderOutPurchase = new Dictionary<string, decimal>();
            foreach (var item in documents.Documents)
            {
                var data = await purchasesFirebase.Document(item.Id).GetSnapshotAsync();
                if (!data.Exists)
                    continue;

                var Items = data.ConvertTo<Dictionary<string, List<Item>>>();
                var products = Items["Purcheses"];
                orderOutPurchase.Add(item.Id, (decimal)products.Sum(x => x.Price));
            }

            var orderPurchases = new Dictionary<string, DateTime>();
            foreach (var item in orderOutPurchase)
            {
                var data = item.Key.Split("-").ToList();
                var dataTime = Convert.ToDateTime($"{data[1]}-{data[0]}-01");
                orderPurchases.Add(item.Key, dataTime);
            }

            var mouthPurchase = orderPurchases.OrderBy(x => x.Value).Select(x => x.Key).ToList();
            var purchases = new Dictionary<string, decimal>();
            foreach (var item in mouthPurchase)
            {
                var orderPurchase = orderOutPurchase.FirstOrDefault(x => x.Key ==  item);
                purchases.Add(orderPurchase.Key,orderPurchase.Value);
            }

            return purchases;
        }
    }
}