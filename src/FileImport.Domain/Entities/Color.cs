using System.Collections.Generic;

namespace FileImport.Domain.Entities
{
    public class Color : BaseEntity
    {
        public Color()
        {
            Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public ICollection<Product> Products { get; private set; }
    }
}
