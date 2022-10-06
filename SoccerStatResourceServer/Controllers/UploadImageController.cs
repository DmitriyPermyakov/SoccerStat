using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerStatResourceServer.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatResourceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController: ControllerBase
    {
        private IUploadImage imageService;        
        public UploadImageController(IUploadImage imageService)
        {
            this.imageService = imageService;            
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFileAsync()
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest("No files found in the request");
            if (Request.Form.Files.Count > 1)
                return BadRequest("Cannot upload more than one file at a time");

            if (Request.Form.Files[0].Length <= 0)
                return BadRequest("Invalid file lenght, seems to be empty");

            try
            {
                var file = Request.Form.Files[0];
                string location = await imageService.UploadImageAsync(file);
                string path = "https://localhost:6001/" + location;
                return Ok(new { url = path });
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Upload failed " + ex.Message);
            }
        }        
    }
}
