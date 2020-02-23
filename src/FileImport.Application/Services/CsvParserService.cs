using FileImport.Application.Interfaces;
using FileImport.Application.Mapping;
using FileImport.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace FileImport.Application.Services
{
    public class CsvParserService : ICsvParserService
    {
        private readonly ILogger<CsvParserService> _logger;

        public CsvParserService(ILogger<CsvParserService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<CsvItemModel>> Parse(string filePath)
        {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvItemMapping = new CsvItemMapping();
            var csvParser = new CsvParser<CsvItemModel>(csvParserOptions, csvItemMapping);

            try
            {
                var csvMappingresult = await Task.Run(
                    () => csvParser
                    .ReadFromFile(filePath, Encoding.UTF8)
                    .ToList());

                return csvMappingresult.Where(x => x.IsValid)
                    .Select(x => x.Result)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
