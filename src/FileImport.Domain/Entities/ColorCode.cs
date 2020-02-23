using Newtonsoft.Json;
using System.Collections.Generic;

namespace FileImport.Domain.Entities
{
    public class ColorCode : BaseEntity
    {
        public ColorCode()
        {
            Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; private set; }
    }
}
