using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services.Dtos;
using RM.UI.Api.Helpers;

namespace RM.UI.Api.Controllers
{
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [Authorize]
        [Route("api/purchase/file"), HttpPost]
        public async Task<IActionResult> AddProductAsync(IFormFile file)
        {
            try
            {
                string filePath = await Utilities.CreateFileTemp(file);
                await _purchaseService.AddPurchaseAsync(filePath);
                return Ok(new { path = filePath });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new {message = ex.Message});    
            }
        }

        [Authorize]
        [Route("api/purchase/list"), HttpPost]
        public async Task<IActionResult> AddProductAsync(List<ProductDto> productsDto)
        {
            try
            {                
                var result = await _purchaseService.AddPurchaseAsync(productsDto);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}