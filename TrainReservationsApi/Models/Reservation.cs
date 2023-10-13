/// <summary> 
/// The Reservation class represents a train reservation record in the MongoDB database. 
/// </summary>
/// <author>IT19051758</author>

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainReservationsApi.Models;
public class Reservation
{
    // Id corresponds to _id field in MongoDB and its data type is ObjectId in. 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Departure station of the reservation
    public string departure { get; set; } = null!;

    // Arrival station of the reservation
    public string arrival { get; set; } = null!;

    // Date of train reservation 
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime date { get; set; }

    // Time of train departure
    public string time { get; set; } = null!;

    // Number of tickets reserved
    public int ticketCount { get; set; }

    // Class of tickets reserved
    public int ticketClass { get; set; }

    // Id of the train reserved
    [BsonRepresentation(BsonType.ObjectId)]
    public string trainId { get; set; } = null!;

    // Id of the traveler
    [BsonRepresentation(BsonType.ObjectId)]
    public string travelerId { get; set; } = null!;

}

