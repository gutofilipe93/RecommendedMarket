using System.Collections.Generic;
using System.IO;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [Route("api/product/file"), HttpPost]
        public async Task<IActionResult> AddProductAsync(IFormFile file)
        {
            try
            {
                string filePath = await Utilities.CreateFileTemp(file);
                await _productService.AddProductsAndSearchableNamesAsync(filePath);
                return Ok(new { path = filePath });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new {message = ex.Message});    
            }
        }

        [Authorize]
        [Route("api/product/list"), HttpPost]
        public async Task<IActionResult> AddProductListAsync(List<ProductDto> productsDto)
        {
            try
            {                
                var result = await _productService.AddProductsAndSearchableNamesListAsync(productsDto);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [Route("api/products/searchablenames"), HttpGet]
        public async Task<IActionResult> GetSearchableNamesAsync()
        {
            try
            {                
                var names = await _productService.GetSearchableNamesAsync();
                return Ok(names);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}