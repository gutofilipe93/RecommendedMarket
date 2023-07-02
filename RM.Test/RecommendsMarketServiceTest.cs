using Moq;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RM.Test
{
    public class RecommendsMarketServiceTest
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IRecommendsMarketRepository> _recommendsMarketRepositoryMock;
        private readonly IRecommendsMarketService _recommendsMarketService;
        private readonly List<Product> _products;
        private readonly List<string> _searchableNames;
        private readonly RecommendsMarket _recommendsMarket;
        private readonly List<string> _request;
        public RecommendsMarketServiceTest()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _products = new List<Product>
            {
                new Product
            {
                Name = "Agua",
                Market = "tonin",
                Price = 2.01d,
                PenultimatePrice = 2.05d,
                SearchableName = "agua",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2022",
                DatePenultimatePurchase = "26/08/2020"
            },
            new Product
            {
                Name = "Linguica",
                Market = "tonin",
                Price = 10.05d,
                PenultimatePrice = 10.05d,
                SearchableName = "linguica",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            },
               new Product
            {
                Name = "Agua",
                Market = "savegnago",
                Price = 1.95d,
                PenultimatePrice = 2.07d,
                SearchableName = "agua",
                TemOferta = true,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            },
            new Product
            {
                Name = "Linguica",
                Market = "savegnago",
                Price = 11.05d,
                PenultimatePrice = 9.95d,
                SearchableName = "linguica",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            },
               new Product
            {
                Name = "Iorgute",
                Market = "tonin",
                Price = 1.54d,
                PenultimatePrice = 2.07d,
                SearchableName = "iorgute",
                TemOferta = false,
                DateOfLastPurchase = "26/08/2020",
                DatePenultimatePurchase = "26/08/2020"
            },
            };

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<ICollection<Product>>(_products));

            _searchableNames = new List<string>();
            _searchableNames.Add("linguica");
            _searchableNames.Add("agua");
            _searchableNames.Add("iorgute");
            _productRepositoryMock.Setup(x => x.GetSearchableNamesAsync()).Returns(Task.FromResult<ICollection<string>>(_searchableNames));

            _productRepositoryMock.Setup(x => x.AddAsync(new List<Product>(), "")).Returns(Task.FromResult(new { }));
            _productRepositoryMock.Setup(x => x.AddSearchableNamesAsync(new List<string>())).Returns(Task.FromResult(new { }));
            _productRepositoryMock.Setup(x => x.GetMarkets()).Returns(Task.FromResult<ICollection<string>>(new List<string>{ "tonin", "savegnago"}));

            _recommendsMarket = new RecommendsMarket { Items = new List<RecommendsMarketItem>() };
            _recommendsMarketRepositoryMock = new Mock<IRecommendsMarketRepository>();
            _recommendsMarketRepositoryMock.Setup(x => x.GetRecommendsMarket()).Returns(Task.FromResult<RecommendsMarket>(_recommendsMarket));
            _recommendsMarketRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<RecommendsMarket>())).Returns(Task.FromResult(new { }));

            _recommendsMarketService = new RecommendsMarketService(_productRepositoryMock.Object,_recommendsMarketRepositoryMock.Object);

            _request = new List<string>();
            _request.Add("linguica");
            _request.Add("agua");
            _request.Add("coca-cola");
            _request.Add("iorgute");
        }

        [Fact(DisplayName = "Deve retornar o melhor mecardo para ir e os preço dos produtos")]
        [Trait("Recomendacao de Mercado", "Melhor mercado")]
        public async Task MustReturnTheBestMarketToPurchase()
        {
            _recommendsMarket.Items.Add(new RecommendsMarketItem { Market = "tonin", Name = "Linguica", Price = 9.05d, SearchableName = "linguica" });
            _recommendsMarket.Items.Add(new RecommendsMarketItem { Market = "savegnago", Name = "Iorgute", Price = 1.34d, SearchableName = "iorgute" });
            _recommendsMarket.Items.Add(new RecommendsMarketItem { Market = "savegnago", Name = "Agua", Price = 1.64d, SearchableName = "agua", DateLastPurchase = "2019-02-02"});

            var result = await _recommendsMarketService.GetRecommendsMarket(_request);
            
            Assert.Equal(3, result.Items.Count);
            Assert.Equal("savegnago", result.Market);
            Assert.Equal(9.05d, result.Items.FirstOrDefault(x => x.SearchableName == "linguica").Price);
            Assert.Equal(1.34d, result.Items.FirstOrDefault(x => x.SearchableName == "iorgute").Price);
            Assert.Equal(1.64d, result.Items.FirstOrDefault(x => x.SearchableName == "agua").Price);  
            Assert.Equal(12.03d, Math.Round(result.TotalPrice,2, MidpointRounding.AwayFromZero));          
        }

        [Fact(DisplayName = "Deve retornar o melhor mecardo para ir e os preço dos produtos com o arquivo já criado")]
        [Trait("Recomendacao de Mercado", "Melhor mercado")]
        public async Task MustReturnTheBestMarketToPurchaseOfFile()
        {

            var result = await _recommendsMarketService.GetRecommendsMarket(_request);

            Assert.Equal(3, result.Items.Count);
            Assert.Equal("tonin", result.Market);
            Assert.Equal(10.05d, result.Items.FirstOrDefault(x => x.SearchableName == "linguica").Price);
            Assert.Equal(1.54d, result.Items.FirstOrDefault(x => x.SearchableName == "iorgute").Price);
            Assert.Equal(2.01d, result.Items.FirstOrDefault(x => x.SearchableName == "agua").Price);
            Assert.Equal(13.6d, Math.Round(result.TotalPrice,2, MidpointRounding.AwayFromZero));          
        }
    }
}
