using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace beadando_szoftech.Models
{
    [BsonIgnoreExtraElements]
    public class House
    {
        [BsonId]
        public int id { get; set; }

        public string? date_added { get; set; } = null!;

        public string? date_sold { get; set; } = null!;

        public string? boughtBy { get; set; } = null!;

        public int? price { get; set; } = null!;

        public int? size { get; set; } = null!;

        public string? county { get; set; } = null!;

        public string? city { get; set; } = null!;

        public string? address { get; set; } = null!;

        public string? addedBy { get; set; } = null!;
    }
}
