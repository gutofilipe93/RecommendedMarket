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

        public async Task<ResponseApiHelper> AddProductsAndSearchableNamesAysnc(string file)
        {
            var productsFile = _fileCsvService.ReadFile(file);
            string market = productsFile.FirstOrDefault(x => x.Mercado != null)?.Mercado;
            var productsFirebase = await _productRepository.GetAsync(market);            
            List<Product> products = AddProductsFirebase(productsFile, productsFirebase);
            products = products.Union(AddProductsFile(productsFile, productsFirebase)).ToList();
            var namesFirebase = await AddSearchableNamesAsync(productsFile);
            await _productRepository.AddAsync(products, market);
            await _productRepository.AddSearchableNamesAsync(namesFirebase.ToList());
            return new ResponseApiHelper { Message = "Produtos cadastrados", Success = true, Data = products };
        }

        private List<Product> AddProductsFirebase(List<ProductDto> productsFile, ICollection<Product> productsFirebase)
        {
            List<Product> products = new List<Product>();
            foreach (var product in productsFirebase)
            {
                if (!productsFile.Any(x => x.NomePesquisa == product.SearchableName))
                    products.Add(product);
            }
            return products;
        }

        private List<Product> AddProductsFile(List<ProductDto> productsFile, ICollection<Product> productsFirebase)
        {
            List<Product> products = new List<Product>();
            foreach (var productFile in productsFile)
            {
                Product product = AssembleProductsToList(productsFirebase, productFile);
                products.Add(product);
            }
            return products;
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
                DatePenultimatePurchase = productFile.DataCompra,
                Price = productFile.Preco,
                TemOferta = productFile.TemOferta == 1 ? true : false,
                SearchableName = productFile.NomePesquisa
            };
        }

        private void FormatProductFound(ProductDto productFile, Product productFirebase)
        {
            productFirebase.DatePenultimatePurchase = productFirebase.DateOfLastPurchase;
            productFirebase.PenultimatePrice = productFirebase.Price;
            productFirebase.Price = productFile.Preco;
            productFirebase.DateOfLastPurchase = productFile.DataCompra;
            productFirebase.TemOferta = productFile.TemOferta == 1 ? true : false;
        }

        private async Task<List<string>> AddSearchableNamesAsync(List<ProductDto> productsFile)
        {
            var namesFirebase = await _productRepository.GetSearchableNamesAsync();
            foreach (var productFile in productsFile)
            {
                if (!namesFirebase.Contains(productFile.NomePesquisa))
                    namesFirebase.Add(productFile.NomePesquisa);
            }
            return namesFirebase.OrderBy(x=> x).ToList();
        }
    }
}