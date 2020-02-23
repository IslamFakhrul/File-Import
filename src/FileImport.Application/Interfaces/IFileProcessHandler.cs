using FileImport.Application.Models;
using System.Threading.Tasks;

namespace FileImport.Application.Interfaces
{
    public interface IFileProcessHandler
    {
        Task<ImportFileRequestResponse> Handle(string filePath);
    }
}
