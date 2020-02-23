using FileImport.Domain;
using FileImport.Persistence.MultipleStorageServices.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace FileImport.Persistence.Json
{
    public class JsonDataStorageService : IDataStorageService
    {
        private readonly ILogger<JsonDataStorageService> _logger;
        private readonly FileSettings _fileSettings;

        public JsonDataStorageService(ILogger<JsonDataStorageService> logger, IOptions<FileSettings> fileSettingsAccessor)
        {
            _logger = logger;
            _fileSettings = fileSettingsAccessor.Value;
        }

        public async Task BulkInsertAsync(NormalizedCsvModel normalizedCsvModel)
        {
            _logger.LogInformation("Convert list of object to json.");
            var serializeCsvData = JsonConvert.SerializeObject(normalizedCsvModel);
            
            _logger.LogInformation("Write serialized data to json file.");
            await File.WriteAllTextAsync($"{_fileSettings.JsonFilePath}\\NormalizedCSVData.json", serializeCsvData);
        }
    }
}
