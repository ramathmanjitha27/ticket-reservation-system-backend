using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainReservationsApi.Models
{
    public class Staff
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        //public string? Role { get; set; }
        public List<string>? Roles { get; set; }
        public bool? IsActivated { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        //public string TravelerId { get; set; } = null!;
        public List<string> TravelerIds { get; set; } = new List<string>(); 
    }
}
