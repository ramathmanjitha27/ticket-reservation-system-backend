

using MongoDB.Bson.Serialization.Attributes;


using MongoDB.Bson;
/// <summary> 
/// The Availability class represents a train avaialability search criteria. 
/// </summary>
/// <author>IT19051758</author>


namespace TrainReservationsApi.Models
{
    public class AvailabilitySearch
    {
        // Departure station of the reservation
        public string departure { get; set; } = null!;

        // Arrival station of the reservation
        public string arrival { get; set; } = null!;

        // Date of train reservation 
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public string date { get; set; } = null!;

        // Class of tickets reserved
        public string ticketClass { get; set; } = null!;

        // Number of tickets reserved
        public int ticketCount { get; set; }

    }
}
