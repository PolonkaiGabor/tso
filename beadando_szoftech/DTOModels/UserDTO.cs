using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace beadando_szoftech.DTOModels
{
    public class UserDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Username { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Role { get; set; } = null!;
        public string? JoinDate { get; set; } = null!;
        public string? LastOnlineDate { get; set; } = null!;
    }
}
