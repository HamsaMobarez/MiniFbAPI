using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniFB.BAL.FileUploadManager
{
    public class FileUpload : IFileUpload
    {
        private const int ImageMinBytes = 512;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadImage(IFormFile image, string host)
        {
            if (CheckIfImageFile(image))
            {
                return await WriteFile(host, image, "Images");
            }

            return null;
        }

        public async Task<string> WriteFile(string host, IFormFile file, string folder)
        {
            string path;
            string dataBasePath;
            try
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string contentRootPath = webHostEnvironment.ContentRootPath;

                Logger.Info("webRootPath:{0}, contentRootPath: {1}", webRootPath, contentRootPath);

                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(webRootPath, folder, fileName);
                dataBasePath = Path.Combine(host, folder, fileName);

                Logger.Info("path:{0}, dataBasePath: {1}", path, dataBasePath);

                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        using (MagickImage image = new MagickImage(fileBytes))
                        {
                            image.Format = image.Format; // Get or Set the format of the image.
                            image.Resize(250, 250); // fit the image into the requested width and height. 
                            image.Quality = 200; // This is the Compression level.
                            image.Write(path);
                        }
                    }
                }



            }
            catch (System.Exception e)
            {
                Logger.Error(e, "upload exception");
                throw e;
            }

            return dataBasePath;
        }

        private bool CheckIfImageFile(IFormFile image)
        {
          
            if (image.ContentType.ToLower() != "image/jpg" &&
                        image.ContentType.ToLower() != "image/jpeg" &&
                        image.ContentType.ToLower() != "image/pjpeg" &&
                        image.ContentType.ToLower() != "image/gif" &&
                        image.ContentType.ToLower() != "image/x-png" &&
                        image.ContentType.ToLower() != "image/png")
            {
                return false;
            }
            if (Path.GetExtension(image.FileName).ToLower() != ".jpg"
                && Path.GetExtension(image.FileName).ToLower() != ".png"
                && Path.GetExtension(image.FileName).ToLower() != ".gif"
                && Path.GetExtension(image.FileName).ToLower() != ".jpeg")
            {
                return false;
            }
            try
            {
                if (!image.OpenReadStream().CanRead)
                {
                    return false;
                }
               
                if (image.Length < ImageMinBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinBytes];
                image.OpenReadStream().Read(buffer, 0, ImageMinBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}

