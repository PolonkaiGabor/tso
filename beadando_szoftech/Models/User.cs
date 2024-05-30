using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace beadando_szoftech.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        public string? username { get; set; } = null!;

        public byte[]? passwordHash { get; set; } = null!;

        public byte[]? passwordSalt { get; set; } = null!;

        public string? email { get; set; } = null!;

        public string? role { get; set; } = null!;

        public string? joinDate { get; set; } = null!;

        public string? lastOnlineDate { get; set; } = null!;
    }
}
