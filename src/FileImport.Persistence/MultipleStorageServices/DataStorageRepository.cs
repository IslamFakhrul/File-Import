using FileImport.Domain;
using System;
using System.Threading.Tasks;

namespace FileImport.Persistence.MultipleStorageServices
{
    public class DataStorageRepository : IDataStorageRepository
    {
        private readonly Func<string, IDataStorageService> _storage;
        public DataStorageRepository(Func<string, IDataStorageService> storage)
        {
            _storage = storage;
        }

        public async Task SaveNormalizedItemsAsync(NormalizedCsvModel normalizedCsvModel, StorageTypeEnum storageType)
        {
            await _storage(storageType.ToString()).BulkInsertAsync(normalizedCsvModel);
        }
    }
}
