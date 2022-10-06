using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SoccerStatResourceServer.Services
{
    public interface IUploadImage
    {
        public Task<string> UploadImageAsync(IFormFile file);
    }
}
