using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Domain.Interfaces.Services;

namespace RM.UI.Api.Controllers
{
    public class RecommendsMarketController : Controller
    {
        private readonly IRecommendsMarketService _recommendsMarketService;
        public RecommendsMarketController(IRecommendsMarketService recommendsMarketService)
        {
            _recommendsMarketService = recommendsMarketService;
        }

        [Authorize]
        [Route("api/recommendsmarket"), HttpPost]
        public async Task<IActionResult> GetRecommendsMarketAsync([FromBody] List<string> items)
        {
            try
            {  
                var result = await _recommendsMarketService.GetRecommendsMarket(items);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new {message = ex.Message});    
            }
        }

        [Authorize]
        [Route("api/products/market/{market}"), HttpPost]
        public async Task<IActionResult> GetProductsByMarket([FromBody] List<string> items, string market)
        {
            try
            {  
                var result = await _recommendsMarketService.GetProductsByMarket(items,market);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new {message = ex.Message});    
            }
        }
    }
}