using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RM.UI.Api.Helpers
{
    public static class Utilities
    {
        public static async Task<string> CreateFileTemp(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);
            return filePath;
        }
    }
}