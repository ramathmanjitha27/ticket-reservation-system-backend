using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainReservationsApi.Models
{
    public class Traveler
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string NIC { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
