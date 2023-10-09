
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TrainReservationsApi.Models
{
    public class Train
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ? id { get; set; }
    public string name { get; set; }
    public string departureStation { get; set; }
    public string arrivalStation { get; set; }
    public DateTime date { get; set; }
    public bool isActive { get; set; }
    public bool isPublished { get; set; }
    public string departureTime { get; set; }
    public string arrivalTime { get; set; }
    public List<string> availableDates { get; set; }
    public List<TicketAvailability>? ticketsAvailability { get; set; }
    public List<Schedule>? schedules { get; set; }
}

public class TicketAvailability
{
    public string trainClass { get; set; }
    public int tickets { get; set; }
    public int reserved { get; set; }
}

public class Schedule
{
    public string station { get; set; }
    public string arrivalTime { get; set; }
    public string departureTime { get; set; }
}

}
