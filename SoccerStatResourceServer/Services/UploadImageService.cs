using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SoccerStatResourceServer.Services
{
    public class UploadImageService: IUploadImage
    {
        private readonly IWebHostEnvironment env;
        public UploadImageService(IWebHostEnvironment environment)
        {
            this.env = environment;
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            string webRootPath = env.WebRootPath;
            string uploadsDir = Path.Combine(webRootPath, "uploads");
            if(!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string fullPath = Path.Combine(uploadsDir, fileName);

            
            using(var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
            }
            

            string location = $"images/{fileName}";
            return location;
        }
    }
}
