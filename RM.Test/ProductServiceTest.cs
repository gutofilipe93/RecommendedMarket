using Moq;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services;
using RM.Domain.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RM.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IFileCsvService> _fileCsvServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IProductService _productService;
        private readonly List<Product> _products;
        private readonly List<string> _searchableNames;
        private readonly Mock<IPurchaseService> _purchaseServiceMock;

        private readonly Mock<IRecommendsMarketRepository> _recommendsMarketRepositoryMock;
        public ProductServiceTest()
        {
            _fileCsvServiceMock = new Mock<IFileCsvService>();
            _fileCsvServiceMock.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(new List<ProductDto>
            {
                new ProductDto
                {
                    Cpf = "3712144284",
                    Mercado = "tonin",
                    Nome = "Linguiça Sadia",
                    NomePesquisa = "linguica",
                    Preco = 10.11d,
                    TemOferta = 0,
                    DataCompra = "30/08/2020"
                }
            });

            _productRepositoryMock = new Mock<IProductRepository>();
            _products = new List<Product>();
            _products.Add(new Product
            {
                Name = "Agua",
                Market = "tonin",
                Price = 2.05d,
                PenultimatePrice = 2.05d,
                SearchableName = "agua",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });
            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<ICollection<Product>>(_products));
            
            _searchableNames = new List<string>();
            _searchableNames.Add("linguica");
            _searchableNames.Add("agua");
            _productRepositoryMock.Setup(x => x.GetSearchableNamesAsync()).Returns(Task.FromResult<ICollection<string>>(_searchableNames));

            _productRepositoryMock.Setup(x => x.AddAsync(new List<Product>(), "")).Returns(Task.FromResult(new { }));
            _productRepositoryMock.Setup(x => x.AddSearchableNamesAsync(new List<string>())).Returns(Task.FromResult(new { }));

            _recommendsMarketRepositoryMock =new Mock<IRecommendsMarketRepository>();
            _recommendsMarketRepositoryMock.Setup(x => x.DeleteAsync()).Returns(Task.CompletedTask);
            _purchaseServiceMock = new Mock<IPurchaseService>();

            _productService = new ProductService(_fileCsvServiceMock.Object, _productRepositoryMock.Object,_recommendsMarketRepositoryMock.Object,_purchaseServiceMock.Object);
        }

        [Fact(DisplayName = "Deve adicionar os produtos sem duplica-los")]
        [Trait("Recomendacao de Mercado", "Salvar produtos")]
        public async Task MustInsertProductWithoutPublishing()
        {
            _products.Add(new Product
            {
                Name = "Linguiça Sadia",
                Market = "tonin",
                Price = 12.55d,
                PenultimatePrice = 12.55d,
                SearchableName = "linguica",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });

            var result = await _productService.AddProductsAndSearchableNamesAsync("tonin");
            var productsFirebase = (List<Product>)result.Data;
            
            Assert.Equal(2,productsFirebase.Count);
            Assert.Equal(1,productsFirebase.Count(x => x.SearchableName == "linguica"));
            Assert.Equal("30/08/2020",productsFirebase.FirstOrDefault(x => x.SearchableName == "linguica").DateOfLastPurchase);
        }

             
        [Fact(DisplayName = "Deve atualizar o preço e a data de acordo com o arquivo csv")]
        [Trait("Recomendacao de Mercado", "Update preco e data")]
        public async Task MustUpdateDateOfLastPurchaseAndPrice()
        {
            _products.Add(new Product
            {
                Name = "Linguiça Sadia",
                Market = "tonin",
                Price = 12.55d,
                PenultimatePrice = 12.55d,
                SearchableName = "linguica",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });

            var result = await _productService.AddProductsAndSearchableNamesAsync("tonin");
            var productsFirebase = (List<Product>)result.Data;

            Assert.Equal("30/08/2020",productsFirebase.FirstOrDefault(x => x.SearchableName == "linguica").DateOfLastPurchase);
            Assert.Equal("26/08/2020",productsFirebase.FirstOrDefault(x => x.SearchableName == "linguica").DatePenultimatePurchase);
            Assert.Equal(10.11d,productsFirebase.FirstOrDefault(x => x.SearchableName == "linguica").Price);
            Assert.Equal(12.55,productsFirebase.FirstOrDefault(x => x.SearchableName == "linguica").PenultimatePrice);
        }   
        
        [Fact(DisplayName = "Deve inserir os produtos novos e manter os do firebase")]
        [Trait("Recomendacao de Mercado", "Adicionar os novos e manter os outros")]
        public async Task MustAddNewProductsWithoutOverlappingThoseAlreadyRegistered()
        {
            _products.Add(new Product
            {
                Name = "Toddy",
                Market = "tonin",
                Price = 10.55d,
                PenultimatePrice = 10.55d,
                SearchableName = "toddy",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });

            var result = await _productService.AddProductsAndSearchableNamesAsync("tonin");
            var productsFirebase = (List<Product>)result.Data;

            Assert.Equal(3,productsFirebase.Count);
            Assert.Equal(1,productsFirebase.Count(x => x.SearchableName == "linguica"));
            Assert.Equal(1,productsFirebase.Count(x => x.SearchableName == "agua"));
            Assert.Equal(1,productsFirebase.Count(x => x.SearchableName == "toddy"));
        }

        [Fact(DisplayName = "Deve corrigir os nome errados do firebase")]
        [Trait("Recomendacao de Mercado","Ajuste de nomes duplicados ou errados")]
        public async Task MustUpdateNamesErrorsInFirebase()
        {
            ICollection<string> markets = new List<string> { "tonin" };
            _productRepositoryMock.Setup(x => x.GetMarkets()).ReturnsAsync(markets);

            _products.Add(new Product
            {
                Name = "Coca-Cola",
                Market = "tonin",
                Price = 2.05d,
                PenultimatePrice = 2.05d,
                SearchableName = "coca",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });
            _searchableNames.Add("coca");

            var namesDuplicate = new List<DuplicateName>()
            {
                new DuplicateName
                {
                    Error = "coca",
                    Correct= "coca-cola"
                }
            };

            var result = await _productService.AdjustDuplicateNames(namesDuplicate);

            Assert.Equal("coca-cola", _products.FirstOrDefault(x => x.SearchableName == "coca-cola").SearchableName);
            Assert.Equal("coca-cola", _searchableNames.FirstOrDefault(x => x == "coca-cola"));
        }

        [Fact(DisplayName = "Deve corrigir os nome errados do firebase em mais de um mercado")]
        [Trait("Recomendacao de Mercado", "Ajuste de nomes duplicados ou errados")]
        public async Task MustUpdateNamesErrorsInFirebaseInAmountOfOneMarket()
        {
            ICollection<string> markets = new List<string> { "tonin", "big" };
            _productRepositoryMock.Setup(x => x.GetMarkets()).ReturnsAsync(markets);

            _products.Add(new Product
            {
                Name = "Coca-Cola",
                Market = "tonin",
                Price = 2.05d,
                PenultimatePrice = 2.05d,
                SearchableName = "coca",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });
            _products.Add(new Product
            {
                Name = "Coca-Cola",
                Market = "big",
                Price = 2.05d,
                PenultimatePrice = 2.05d,
                SearchableName = "coca",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            });
            _searchableNames.Add("coca");
            _searchableNames.Add("coca-cola");

            var namesDuplicate = new List<DuplicateName>()
            {
                new DuplicateName
                {
                    Error = "coca",
                    Correct= "coca-cola"
                }
            };

            var result = await _productService.AdjustDuplicateNames(namesDuplicate);

            Assert.Equal("coca-cola", _products.FirstOrDefault(x => x.Market == "big" && x.SearchableName == "coca-cola").SearchableName);
            Assert.Equal(3, _searchableNames.Count());
        }
    }
}
