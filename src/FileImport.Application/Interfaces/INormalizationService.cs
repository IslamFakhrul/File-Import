using FileImport.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImport.Application.Interfaces
{
    public interface INormalizationService
    {
        Task<NormalizedCsvModel> Normalize(IEnumerable<CsvItemModel> csvItems);
    }
}
