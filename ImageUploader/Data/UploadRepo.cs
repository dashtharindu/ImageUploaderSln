using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploader.Data
{
    public class UploadRepo
    {
        public static IWebHostEnvironment _environment;//needs using Microsoft.AspNetCore.Hosting;

        public UploadRepo(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> ImageUploader(IFormFile Imagefile)
        {
            try
            {
                if (Imagefile.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + Imagefile.FileName))
                    {
                        await Imagefile.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                        return "/uploads/" + Imagefile.FileName.ToString();
                    }
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }

        }
       
    }
}
