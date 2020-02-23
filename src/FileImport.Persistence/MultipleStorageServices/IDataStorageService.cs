using FileImport.Domain;
using System.Threading.Tasks;

namespace FileImport.Persistence.MultipleStorageServices
{
    public interface IDataStorageService
    {
        Task BulkInsertAsync(NormalizedCsvModel normalizedCsvModel);
    }
}
