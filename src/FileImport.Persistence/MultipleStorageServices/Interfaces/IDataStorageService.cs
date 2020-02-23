using FileImport.Domain;
using System.Threading.Tasks;

namespace FileImport.Persistence.MultipleStorageServices.Interfaces
{
    public interface IDataStorageService
    {
        Task BulkInsertAsync(NormalizedCsvModel normalizedCsvModel);
    }
}
