using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainReservationsApi.Models
{
    public class Staff
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        // The unique identifier for a staff member, stored as an ObjectId in the MongoDB collection.
        public string? Username { get; set; }
        // The username associated with the staff member.
        public string? Email { get; set; }
        // The email address of the staff member.
        public string? Password { get; set; }
        // The password for the staff member
        public string? FullName { get; set; }
        // The full name of the staff member.

        public List<string>? Roles { get; set; }
        // A list of roles assigned to the staff member. Roles define their permissions and access.
        public bool? IsActivated { get; set; }
        // A flag indicating whether the staff member's account is activated or deactivated.

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> TravelerIds { get; set; } = new List<string>();
        // A list of unique identifiers (ObjectIds) of travelers associated with the staff member.
    }
}
