using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainReservationsApi.Models;

namespace TrainReservationsApi.Services
{
    public class TrainService
    {
        private readonly IMongoCollection<Train> _trainsCollection;

        public TrainService(
            IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                TrainReservationsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                TrainReservationsDatabaseSettings.Value.DatabaseName);

            _trainsCollection = mongoDatabase.GetCollection<Train>(
                TrainReservationsDatabaseSettings.Value.TrainsCollectionName);
        }

        public async Task<List<Train>> GetAllAsync() =>
            await _trainsCollection.Find(_ => true).ToListAsync();

        public async Task<Train?> GetByIdAsync(string id) =>
            await _trainsCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Train newTrain) =>
            await _trainsCollection.InsertOneAsync(newTrain);

        public async Task UpdateAsync(string id, Train updatedTrain) =>
            await _trainsCollection.ReplaceOneAsync(x => x.id == id, updatedTrain);

        public async Task DeleteAsync(string id) =>
            await _trainsCollection.DeleteOneAsync(x => x.id == id);
    }

}
