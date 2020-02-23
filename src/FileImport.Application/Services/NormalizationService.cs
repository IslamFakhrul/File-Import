using FileImport.Application.Interfaces;
using FileImport.Domain;
using FileImport.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileImport.Application.Services
{
    public class NormalizationService : INormalizationService
    {
        private readonly ILogger<NormalizationService> _logger;

        public NormalizationService(ILogger<NormalizationService> logger)
        {
            _logger = logger;
        }

        public async Task<NormalizedCsvModel> Normalize(IEnumerable<CsvItemModel> csvItems)
        {
            try
            {
                var colors = ExtractColors(csvItems);
                var colorCodes = ExtractColorCodes(csvItems);

                var products = csvItems.Select((product, index) => new Product
                {
                    Id = index + 1,
                    Key = product.Key,
                    ArtikelCode = product.ArtikelCode,
                    Description = product.Description,
                    Price = product.Price,
                    DiscountPrice = product.DiscountPrice,
                    DeliveredIn = product.DeliveredIn,
                    Q1 = product.Q1,
                    Size = product.Size,
                    ColorCodeId = colorCodes.Where(cc => cc.Name.ToLowerInvariant() == product.ColorCode.ToLowerInvariant())
                    .Select(cc => cc.Id)
                    .FirstOrDefault(),
                    ColorId = colors.Where(cc => cc.Name.ToLowerInvariant() == product.Color.ToLowerInvariant())
                    .Select(cc => cc.Id)
                    .FirstOrDefault()
                }).ToList();

                return new NormalizedCsvModel
                {
                    Colors = colors,
                    ColorCodes = colorCodes,
                    Products = products
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        private IList<Color> ExtractColors(IEnumerable<CsvItemModel> csvItems)
        {
            var distinctColors = csvItems.Select(x => x.Color).Distinct();

            return distinctColors.Select((color, index) => new Color { Id = index + 1, Name = color })
                .ToList();
        }

        private IList<ColorCode> ExtractColorCodes(IEnumerable<CsvItemModel> csvItems)
        {
            var distinctColorCodes = csvItems.Select(x => x.ColorCode).Distinct();

            return distinctColorCodes.Select((colorCode, index) => new ColorCode { Id = index + 1, Name = colorCode })
                .ToList();
        }
    }
}
