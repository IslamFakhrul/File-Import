using FileImport.Domain;
using System.Threading.Tasks;

namespace FileImport.Persistence.MultipleStorageServices
{
    public interface IDataStorageRepository
    {
        Task SaveNormalizedItemsAsync(NormalizedCsvModel normalizedCsvModel, StorageTypeEnum storageType);
    }
}
