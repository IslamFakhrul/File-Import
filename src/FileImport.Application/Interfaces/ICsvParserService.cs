using FileImport.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImport.Application.Interfaces
{
    public interface ICsvParserService
    {
        Task<IEnumerable<CsvItemModel>> Parse(string filePath);
    }
}
