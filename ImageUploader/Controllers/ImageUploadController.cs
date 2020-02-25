using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageUploader.Data;
using Microsoft.AspNetCore.Hosting; //for IWebHostEnvironment
using Microsoft.AspNetCore.Http; //for IFormFile
using Microsoft.AspNetCore.Mvc;

namespace ImageUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly UploadRepo UploadRepo;

        public ImageUploadController(IWebHostEnvironment environment)
        {
            UploadRepo = new UploadRepo(environment);
        }


        [HttpPost]
        public async Task<IActionResult> Post(IFormFile img)
        {
            try
            {
                string msg = await UploadRepo.ImageUploader(img, 300, 100);
                return Ok(msg);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}