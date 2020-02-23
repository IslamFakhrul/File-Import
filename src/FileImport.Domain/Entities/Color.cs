using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FileImport.Domain.Entities
{
    public class Color : BaseEntity
    {
        public Color()
        {
            Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; private set; }
    }
}
