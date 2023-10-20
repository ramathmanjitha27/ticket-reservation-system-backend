using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainReservationsApi.Models
{
    public class ticketUpdate
    {

        // Class of tickets reserved
        public string ticketClass { get; set; } = null!;      

        // Number of tickets reserved
        public int ticketCount { get; set; }        

        // Id of the train reserved
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;

        // ticket count increment or decrement
        public bool ticketAction { get; set; }

    }
}
