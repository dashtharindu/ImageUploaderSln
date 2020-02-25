using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;

namespace ImageUploader.Data
{
    public class UploadRepo
    {
        public static IWebHostEnvironment _environment;//needs using Microsoft.AspNetCore.Hosting;

        public UploadRepo(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> ImageUploader(IFormFile Imagefile, int width, int height)
        {
            string InputFile = await ImageSave(Imagefile);
            try
            {
                const int quality = 75;

                using (var image = new MagickImage($"{_environment.WebRootPath}/uploads/{InputFile}"))
                {
                    if (image.Height != height || image.Width != width)
                    {
                        decimal result_ratio = (decimal)height / width;
                        decimal current_ratio = (decimal)image.Height / image.Width;

                        bool preserve_width = false;
                        if (current_ratio > result_ratio)
                        {
                            preserve_width = true; //current image's width is bigger than given width
                        }

                        int new_width = 0;
                        int new_height = 0;

                        if (preserve_width) //current image's width is bigger than given width
                        {
                            new_width = width;
                            new_height = (int)Math.Round(current_ratio * new_width);
                        }
                        else //current image's height is bigger than given height
                        {
                            new_height = height;
                            new_width = (int)Math.Round(new_height / current_ratio);
                        }

                        image.Resize(new_width, new_height);
                        //image.Crop(width, height);
                        image.Extent(width, height, Gravity.Center);

                    }

                    //image.Resize(width, height);
                    image.Strip();
                    image.Quality = quality;
                    if (File.Exists($"{_environment.WebRootPath}/magick/{InputFile}"))
                    {
                        throw new Exception("Please try again");
                        //generated new file name is already exist
                    }
                    else
                    {
                        image.Write($"{_environment.WebRootPath}/magick/{InputFile}");
                        return "magick/" + InputFile;
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex == null)
                {
                    throw new Exception("Invalid photo");
                }
                else
                {
                    throw;
                }

            }
            finally
            {
                //delete temp file
                if (File.Exists($"{_environment.WebRootPath}/uploads/{InputFile}"))
                {
                    File.Delete($"{_environment.WebRootPath}/uploads/{InputFile}");
                }
            }

        }

        private async Task<string> ImageSave(IFormFile imagefile)
        {
            try
            {
                List<string> fTypes = new List<string>
                {
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };


                if (imagefile.Length > 0 && fTypes.Contains(imagefile.ContentType))
                {
                    if (!Directory.Exists($"{_environment.WebRootPath}/uploads/"))
                    {
                        Directory.CreateDirectory($"{_environment.WebRootPath}/uploads/");
                    }

                    string newFileName = "";

                    if (imagefile.ContentType == "image/png")
                    {
                        newFileName = $"{Guid.NewGuid().ToString()}.png";
                    }
                    else
                    {
                        newFileName = $"{Guid.NewGuid().ToString()}.jpg";
                    }

                    //string newFileName = "hi";
                    //generate new file name

                    using (var stream = new FileStream(
                        Path.Combine($"{_environment.WebRootPath}/uploads/", newFileName), FileMode.Create))
                    {
                        await imagefile.CopyToAsync(stream);
                        await stream.FlushAsync();

                        return newFileName;
                    }
                }
                else
                {
                    throw new Exception("Invalid photo");
                }
            }
            catch (Exception ex)
            {
                if (ex == null)
                {
                    throw new Exception("Upload failed");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
