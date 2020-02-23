using FileImport.Application.Interfaces;
using FileImport.Application.Models;
using FileImport.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace FileImport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IFileProcessHandler _fileProcessHandler;
        private readonly FileSettings _fileSettings;

        public ImportController(IFileProcessHandler fileProcessHandler, IOptions<FileSettings> fileSettingsAccessor)
        {
            _fileProcessHandler = fileProcessHandler;
            _fileSettings = fileSettingsAccessor.Value;
        }

        [HttpPost("ImportFile")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        [ProducesDefaultResponseType(typeof(ImportFileRequestResponse))]
        public async Task<IActionResult> ImportFileAsync(IFormFile file)
        {
            long size = file.Length;
            string filePath = $"{_fileSettings.CsvFilePath}\\{file.FileName}";

            var response = new ImportFileRequestResponse();

            if (size > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                response = await _fileProcessHandler.Handle(filePath);
            }

            response.FileName = file.FileName;
            response.FileSize = size;

            return Ok(response);
        }
    }
}