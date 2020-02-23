using EFCore.BulkExtensions;
using FileImport.Domain;
using FileImport.Domain.Entities;
using FileImport.Persistence.MultipleStorageServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileImport.Persistence.Database
{
    public class DatabaseStorageService : IDataStorageService
    {
        private readonly FileImportDbContext _dbContext;
        private readonly ILogger<DatabaseStorageService> _logger;

        public DatabaseStorageService(FileImportDbContext dbContext, ILogger<DatabaseStorageService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task BulkInsertAsync(NormalizedCsvModel normalizedCsvModel)
        {
            try
            {
                _logger.LogInformation("Saving Colors.");
                await _dbContext.BulkInsertAsync<Color>(normalizedCsvModel.Colors);

                _logger.LogInformation("Saving ColorCodes.");
                await _dbContext.BulkInsertAsync<ColorCode>(normalizedCsvModel.ColorCodes);

                _logger.LogInformation("Saving Products.");
                await _dbContext.BulkInsertAsync<Product>(normalizedCsvModel.Products);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message, ex);
                throw;
            }
        }
    }
}
