using FileImport.Application.Interfaces;
using FileImport.Application.Models;
using FileImport.Persistence.MultipleStorageServices;
using FileImport.Persistence.MultipleStorageServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileImport.Application.Handler
{
    public class FileProcessHandler : IFileProcessHandler
    {
        private readonly ILogger<FileProcessHandler> _logger;
        private readonly ICsvParserService _csvParserService;
        private readonly INormalizationService _normalizationService;
        private readonly IDataStorageRepository _dataStorageRepository;

        public FileProcessHandler(ICsvParserService csvParserService,
            INormalizationService normalizationService,
            IDataStorageRepository dataStorageRepository,
            ILogger<FileProcessHandler> logger)
        {
            _csvParserService = csvParserService;
            _normalizationService = normalizationService;
            _dataStorageRepository = dataStorageRepository;
            _logger = logger;
        }

        public async Task<ImportFileRequestResponse> Handle(string filePath)
        {
            try
            {
                _logger.LogInformation("Start processing the saved file content...");
                var parsedDataItems = await _csvParserService.Parse(filePath);
                var normalizedCSVData = await _normalizationService.Normalize(parsedDataItems);
                _logger.LogInformation("Finish extracting content from the file.");

                _logger.LogInformation("Start processing content to store different storage...");

                var databaseProcess = _dataStorageRepository.SaveNormalizedItemsAsync(normalizedCSVData, StorageTypeEnum.Database);
                var jsonProcess = _dataStorageRepository.SaveNormalizedItemsAsync(normalizedCSVData, StorageTypeEnum.JSON);

                await Task.WhenAll(databaseProcess, jsonProcess);
                _logger.LogInformation($"Finish storing content in {StorageTypeEnum.Database} and {StorageTypeEnum.JSON}.");

                return new ImportFileRequestResponse
                {
                    ProcessedItems = normalizedCSVData.Products.Count,
                    ResponseMessage = $"Import file processed successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Import file processing failed.");

                return new ImportFileRequestResponse
                {
                    ProcessedItems = 0,
                    ResponseMessage = $"Import file processing failed. Error: {ex.Message}"
                };
            }
        }
    }
}
