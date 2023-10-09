namespace TrainReservationsApi.Models
{
    public class TrainReservationsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ReservationsCollectionName { get; set; } = null!;

        public string? StaffCollectionName { get; set; } = null!;
    }
}
