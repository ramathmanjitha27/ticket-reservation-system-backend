/**
 *the clas of TrainReservationsDatabaseSettings
 *
 * <p>Bugs: None
 *
 * @author All
 */
namespace TrainReservationsApi.Models
{
    public class TrainReservationsDatabaseSettings
    {
        // MongoDB connection URI
        public string ConnectionString { get; set; } = null!;

        // Name of the MongoDB database
        public string DatabaseName { get; set; } = null!;


        // Names of collections
        public string ReservationsCollectionName { get; set; } = null!;

        public string? StaffCollectionName { get; set; } = null!;
        public string TrainsCollectionName { get; set; } = null!;

        public string TravelersCollectionName { get; set; } = null!;
    }
}
