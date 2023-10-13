using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainReservationsApi.Models
{
    // The Train class represents information about a train.
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }  // Unique identifier for the train
        public string name { get; set; }  // The name of the train
        public string departureStation { get; set; }  // Departure station
        public string arrivalStation { get; set; }  // Arrival station
        public bool isActive { get; set; }  // Indicates if the train is active
        public bool isPublished { get; set; }  // Indicates if the train is published
        public string departureTime { get; set; }  // Departure time
        public string arrivalTime { get; set; }  // Arrival time
        public List<string> availableDates { get; set; }  // List of available dates
        public int firstClassTickets { get; set; }  // Number of first class tickets available
        public int secondClassTickets { get; set; }  // Number of second class tickets available
        public int thirdClassTickets { get; set; }  // Number of third class tickets available
        public int? firstClassTicketsReserved { get; set; }  // Number of first class tickets reserved (nullable)
        public int? secondClassTicketsReserved { get; set; }  // Number of second class tickets reserved (nullable)
        public int? thirdClassTicketsReserved { get; set; }  // Number of third class tickets reserved (nullable)
        public List<Schedule>? schedules { get; set; }  // List of schedules for the train
    }

    // The Schedule class represents the schedule of a train, including station, arrival time, and departure time.
    public class Schedule
    {
        public string station { get; set; }  // Station name
        public string arrivalTime { get; set; }  // Arrival time at the station
        public string departureTime { get; set; }  // Departure time from the station
    }

}
