using FileImport.Application.Interfaces;
using FileImport.Application.Models;
using FileImport.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
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
        [RequestSizeLimit(52428800)]
        [ProducesResponseType(typeof(ImportFileRequestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ImportFileRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportFileAsync(IFormFile file)
        {
            var response = new ImportFileRequestResponse();
            var statusCode = (int)HttpStatusCode.OK;

            try
            {
                long size = file.Length;

                if (size <= 0)
                {
                    response.ResponseMessage = "Import file is empty.";
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (fileExtension != "csv")
                {
                    response.ResponseMessage = "Invalid file format.";
                }

                var filePath = $"{_fileSettings.CsvFilePath}\\{file.FileName}";

                if (size > 0 && fileExtension == "csv")
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    response = await _fileProcessHandler.Handle(filePath);
                }

                response.FileName = file.FileName;
                response.FileSize = size;
            }
            catch (Exception ex)
            {
                response = new ImportFileRequestResponse
                {
                    ProcessedItems = 0,
                    ResponseMessage = $"Import file processing failed. Error: {ex.Message}"
                };

                statusCode = (int)HttpStatusCode.BadRequest;
            }

            return new JsonResult(response) { StatusCode = statusCode };
        }
    }
}