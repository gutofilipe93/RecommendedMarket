using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services.Dtos;

namespace RM.Domain.Services
{
    public class RecommendsMarketService : IRecommendsMarketService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRecommendsMarketRepository _recommendsMarketRepository;

        public RecommendsMarketService(IProductRepository productRepository, IRecommendsMarketRepository recommendsMarketRepository)
        {
            _productRepository = productRepository;
            _recommendsMarketRepository = recommendsMarketRepository;
        }

        public async Task<RecommendsMarket> GetRecommendsMarket(List<string> itemsPurchase)
        {
            var recommendsMarketFirebase = await _recommendsMarketRepository.GetRecommendsMarket();
            if (recommendsMarketFirebase.Items.Count == 0)
            {
                var productsFirebase = await MergeProductsByMarket();
                var itemsRecommends = await GetProductsRecommends(productsFirebase);
                recommendsMarketFirebase.Items = itemsRecommends;
                await _recommendsMarketRepository.SaveAsync(recommendsMarketFirebase);
            }
            var recommendsMarket = new RecommendsMarket { Items = new List<RecommendsMarketItem>() };
            Dictionary<string, int> markets = new Dictionary<string, int>();
            foreach (var item in itemsPurchase)
            {
                var product = recommendsMarketFirebase.Items.FirstOrDefault(x => x.SearchableName == item);
                if (product == null)
                    continue;

                recommendsMarket.Items.Add(product);
                GetBestShoppingMarket(markets, product);
            }
            recommendsMarket.Market = markets.OrderByDescending(x => x.Value).FirstOrDefault().Key;
            recommendsMarket.Message = GetAmountMarketByProducts(markets);
            recommendsMarket.TotalPrice =  recommendsMarket.Items.Sum(x => x.Price);
            return recommendsMarket;
        }

        private async Task<List<Product>> MergeProductsByMarket()
        {
            var markets = await _productRepository.GetMarkets();
            var productsFirebase = new List<Product>();
            foreach (var market in markets)
            {
                var productFirebase = await _productRepository.GetAsync(market);
                productsFirebase = productsFirebase.Union(productFirebase).ToList();
            }
            return productsFirebase;
        }

        private async Task<List<RecommendsMarketItem>> GetProductsRecommends(List<Product> productsFirebase)
        {
            var namesSearchable = await _productRepository.GetSearchableNamesAsync();
            var itemsRecommends = new List<RecommendsMarketItem>();
            var dateLimit = DateTime.Now.Subtract(TimeSpan.FromDays(365));
            foreach (var name in namesSearchable)
            {
                Product product = null;
                var products = productsFirebase.Where(x => x.SearchableName == name).ToList();
                if (products.Count() == 0)
                    continue;
                else if (products.Count() == 1)
                    product = products.FirstOrDefault();
                else
                {
                    product = products.OrderBy(x => x.Price).FirstOrDefault(x => ConvertStringToDateTime(x.DateOfLastPurchase) > dateLimit)
                               ?? products.OrderBy(x => x.Price).FirstOrDefault();
                }
                if(product != null)
                    itemsRecommends.Add(FormatProductToRecommendMarkeItem(product));
            }
            return itemsRecommends;
        }

        private DateTime ConvertStringToDateTime(string datePurchase)
        {
            var date = datePurchase.Split('/');
            return Convert.ToDateTime($"{date[2]}-{date[1]}-{date[0]}");
        }

        private RecommendsMarketItem FormatProductToRecommendMarkeItem(Product product)
        {
            return new RecommendsMarketItem
            {
                DateLastPurchase = product.DateOfLastPurchase,
                HaveOffer = product.TemOferta,
                Market = product.Market,
                Name = product.Name,
                Price = product.Price,
                SearchableName = product.SearchableName
            };
        }

        private void GetBestShoppingMarket(Dictionary<string, int> markets, RecommendsMarketItem product)
        {
            if (markets.ContainsKey(product.Market))
            {
                var market = markets.FirstOrDefault(x => x.Key == product.Market);
                int count = market.Value + 1;
                markets.Remove(product.Market);
                markets.Add(product.Market, count);
            }
            else
                markets.Add(product.Market, 1);
        }

        private string GetAmountMarketByProducts(Dictionary<string, int> markets)
        {
            var sortedMarket = from entry in markets orderby entry.Value descending select entry;
            StringBuilder sbMessage = new StringBuilder();
            foreach (var market in sortedMarket)
            {
                sbMessage.Append($"{market.Value} - {market.Key} ; ");
            }

            return sbMessage.ToString();
        }

        public async Task<RecommendsMarket> GetProductsByMarket(List<string> itemsPurchase, string market)
        {
            if(string.IsNullOrEmpty(market))
               return new RecommendsMarket { Message = "Mercado não informado", Items = new List<RecommendsMarketItem>() };

            var productsFirebase = await _productRepository.GetAsync(market);
            var recommendsMarket = new RecommendsMarket { Items = new List<RecommendsMarketItem>() };
            foreach (var item in itemsPurchase)
            {
                var product = productsFirebase.FirstOrDefault(x => x.SearchableName == item);
                if (product == null)
                    continue;

                recommendsMarket.Items.Add(FormatProductToRecommendMarkeItem(product));                
            }
            
            recommendsMarket.Market = market;
            recommendsMarket.TotalPrice =  recommendsMarket.Items.Sum(x => x.Price);
            return recommendsMarket;
        }
    }
}