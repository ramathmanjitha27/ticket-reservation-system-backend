using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainReservationsApi.Models;
public class Reservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string departure { get; set; } = null!;

    public string arrival { get; set; } = null!;

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime date { get; set; }

    public string time { get; set; } = null!;

    public int ticketCount { get; set; }

    public int ticketClass { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string trainId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string travelerId { get; set; } = null!;

}

