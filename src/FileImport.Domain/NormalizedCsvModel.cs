using FileImport.Domain.Entities;
using System.Collections.Generic;

namespace FileImport.Domain
{
    public class NormalizedCsvModel
    {
        public IList<Color> Colors { get; set; }

        public IList<ColorCode> ColorCodes { get; set; }

        public IList<Product> Products { get; set; }
    }
}
