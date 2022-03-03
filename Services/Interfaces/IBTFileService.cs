using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace TheBugTracker.Services.Interfaces
{
    public interface IBTFileService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        public string ConvertByteArrayToFileAsync(byte[] fileData, string extension);
        public string GetFileIcon(string file);
        public string FormatFileSize(long bytes);
    }
}
