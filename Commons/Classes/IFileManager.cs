using Microsoft.AspNetCore.Mvc;

namespace ApiTrial1.Commons.Classes
{
    public interface IFileManager
    {
        Task<string> UploadFile(IFormFile _IFormFile);
        Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}
