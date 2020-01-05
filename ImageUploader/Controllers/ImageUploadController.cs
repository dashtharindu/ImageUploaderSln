using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        //todo-create controller and put this 1

        public static IWebHostEnvironment _environment;

        //constructor
        public ImageUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + file.FileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        return Ok("\\uploads\\" + file.FileName.ToString());
                    }
                }
                else
                {
                    return Ok("failed");
                }
            }
            catch (Exception ex)
            {

                return Ok(ex.Message.ToString());
            }
        }
    }
}