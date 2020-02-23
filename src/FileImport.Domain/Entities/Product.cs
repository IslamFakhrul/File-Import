using Newtonsoft.Json;

namespace FileImport.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Key { get; set; }

        public string ArtikelCode { get; set; }

        public int ColorCodeId { get; set; }

        [JsonIgnore]
        public ColorCode ColorCode { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }

        public string DeliveredIn { get; set; }

        public string Q1 { get; set; }

        public int Size { get; set; }

        public int ColorId { get; set; }

        [JsonIgnore]
        public Color Color { get; set; }

    }
}
