using MongoDB.Bson.Serialization.Attributes;

namespace beadando_szoftech.Models
{
    public class Counter
    {
        [BsonId]
        public string Id { get; set; }
        public int SequenceValue { get; set; }
    }
}
