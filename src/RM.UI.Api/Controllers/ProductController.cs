using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RM.Domain.Interfaces.Services;

namespace RM.UI.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [Route("api/product"), HttpPost]
        public async Task<IActionResult> AddProductAsync(IFormFile file)
        {
            try
            {
                string filePath = await CreateFileTemp(file);
                await _productService.AddProductsAndSearchableNamesAysnc(filePath);
                return Ok(new { path = filePath });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new {message = ex.Message});    
            }
        }

        private async Task<string> CreateFileTemp(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);
            return filePath;
        }
    }
}