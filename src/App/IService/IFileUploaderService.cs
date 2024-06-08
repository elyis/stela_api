using Microsoft.AspNetCore.Mvc;

namespace webApiTemplate.src.App.IService
{
    public interface IFileUploaderService
    {
        Task<IActionResult> UploadFileAsync(string directoryPath, Stream stream, string[] supportedExtensions);
        Task<IActionResult> GetStreamFileAsync(string directoryPath, string filename);
    }
}