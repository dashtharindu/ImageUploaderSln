using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageUploader.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            return Ok(await UploadRepo.ImageUploader(img));
        }
    }
}