namespace TrainReservationsApi.Models
{
    public class TrainReservationsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ReservationsCollectionName { get; set; } = null!;

        public string TrainsCollectionName { get; set; } = null!;
    }
}
