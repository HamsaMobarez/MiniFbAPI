
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiniFB.BAL.FileUploadManager
{
    public interface IFileUpload
    {
        public Task<string> UploadImage(IFormFile image, string host);
    }
}
