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
    public class ProductService : IProductService
    {
        private readonly IFileCsvService _fileCsvService;
        private readonly IProductRepository _productRepository;

        public ProductService(IFileCsvService fileCsvService, IProductRepository productRepository)
        {
            _fileCsvService = fileCsvService;
            _productRepository = productRepository;
        }

        public async Task<ResponseApiHelper> AddProductsAysnc(string file)
        {
            var productsFile = _fileCsvService.ReadFile(file);
            string market = productsFile.FirstOrDefault(x => x.Mercado != null)?.Mercado;
            var productsFirebase = await _productRepository.GetAsync(market);
            List<Product> products = new List<Product>();
            foreach (var productFile in productsFile)
            {
                Product product = AssembleProductsToList(productsFirebase,productFile);
                products.Add(product);
            }
            await _productRepository.AddAsync(products,market);
            return new ResponseApiHelper{Message = "Produtos cadastrados",Success = true, Data = products};
        }

        private Product AssembleProductsToList(ICollection<Product> productsFirebase, ProductDto productsFile)
        {
            var productFirebase = productsFirebase.FirstOrDefault(x => x.SearchableName == productsFile.NomePesquisa);
            if (productFirebase == null)
                productFirebase = FormatProductNew(productsFile);
            else
                FormatProductFound(productsFile, productFirebase);
            return productFirebase;
        }

        private Product FormatProductNew(ProductDto productFile)
        {
            return new Product
            {
                Name = productFile.Nome,
                DateOfLastPurchase = productFile.DataCompra,
                Market = productFile.Mercado,
                PenultimatePrice = productFile.Preco,
                PenultimatePurchaseDate = productFile.DataCompra,
                Price = productFile.Preco,
                TemOferta = productFile.TemOferta == 1 ? true : false,
                SearchableName = productFile.NomePesquisa
            };
        }

        private void FormatProductFound(ProductDto productFile, Product productFirebase)
        {
            productFirebase.PenultimatePurchaseDate = productFirebase.DateOfLastPurchase;
            productFirebase.PenultimatePrice = productFirebase.Price;
            productFirebase.Price = productFile.Preco;
            productFirebase.TemOferta = productFile.TemOferta == 1 ? true : false;
        }
    }
}