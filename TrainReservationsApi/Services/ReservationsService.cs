using TrainReservationsApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TrainReservationsApi.Services
{
    public class ReservationsService
    {
        private readonly IMongoCollection<Reservation> _reservationsCollection;

        public ReservationsService(
            IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                TrainReservationsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                TrainReservationsDatabaseSettings.Value.DatabaseName);

            _reservationsCollection = mongoDatabase.GetCollection<Reservation>(
                TrainReservationsDatabaseSettings.Value.ReservationsCollectionName);
        }

        // Asynchronous function to get all reservations records
        public async Task<List<Reservation>> GetAsync() =>
            await _reservationsCollection.Find(_ => true).ToListAsync();

        // Asynchronous function to get the reservation record of a particular id
        public async Task<Reservation?> GetAsync(string id) =>
            await _reservationsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Asynchronous function to insert a new reservation record
        public async Task CreateAsync(Reservation newReservation) =>
            await _reservationsCollection.InsertOneAsync(newReservation);

        // Asynchronous function to update an existing reservation record
        public async Task UpdateAsync(string id, Reservation updatedReservation) =>
            await _reservationsCollection.ReplaceOneAsync(x => x.Id == id, updatedReservation);

        // Asynchronous function to delete a reservation record
        public async Task RemoveAsync(string id) =>
            await _reservationsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
