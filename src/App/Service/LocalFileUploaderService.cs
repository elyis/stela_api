using Microsoft.AspNetCore.Mvc;

using MimeDetective;

using webApiTemplate.src.App.IService;

namespace webApiTemplate.src.App.Service
{
    public class LocalFileUploaderService : IFileUploaderService
    {
        private readonly ContentInspector _contentInspector;

        public LocalFileUploaderService(ContentInspector contentInspector)
        {
            _contentInspector = contentInspector;
        }

        public async Task<IActionResult> UploadFileAsync(string directoryPath, Stream stream, string[] supportedExtensions)
        {
            try
            {
                if (stream == null || stream.Length == 0)
                    return new BadRequestResult();

                var result = _contentInspector.Inspect(stream);
                var fileExtension = result.MaxBy(e => e.Points)?.Definition.File.Extensions.First();
                if (fileExtension == null || !supportedExtensions.Contains(fileExtension))
                    return new UnsupportedMediaTypeResult();


                string filename = $"{Guid.NewGuid()}.{fileExtension}";
                string fullPathToFile = Path.Combine(directoryPath, filename);

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                using var file = File.Create(fullPathToFile);
                if (file == null)
                    return new BadRequestResult();

                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(file);
                return new OkObjectResult(filename);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetStreamFileAsync(string directoryPath, string filename)
        {
            string fullPathToFile = Path.Combine(directoryPath, filename);
            if (!File.Exists(fullPathToFile))
                return new NotFoundResult();

            using Stream fileStream = File.OpenRead(fullPathToFile);
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);

            var result = memoryStream.ToArray();
            return new FileContentResult(result, $"application/octet-stream");
        }
    }
}