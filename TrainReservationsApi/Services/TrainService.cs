using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainReservationsApi.Models;

namespace TrainReservationsApi.Services
{
    // This service class is responsible for interacting with the MongoDB database
    // to perform CRUD (Create, Read, Update, Delete) operations on Train objects.

    public class TrainService
    {
        private readonly IMongoCollection<Train> _trainsCollection;

        public TrainService(IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            // Initialize the TrainService with database connection settings.

            var mongoClient = new MongoClient(
                TrainReservationsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                TrainReservationsDatabaseSettings.Value.DatabaseName);

            _trainsCollection = mongoDatabase.GetCollection<Train>(
                TrainReservationsDatabaseSettings.Value.TrainsCollectionName);
        }

        // Retrieve all trains from the database.
        public async Task<List<Train>> GetAllAsync() =>
            await _trainsCollection.Find(_ => true).ToListAsync();

        // Retrieve a specific train by its unique ID.
        public async Task<Train?> GetByIdAsync(string id) =>
            await _trainsCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        // Create a new train record in the database.
        public async Task CreateAsync(Train newTrain) =>
            await _trainsCollection.InsertOneAsync(newTrain);

        // Update an existing train's information in the database.
        public async Task UpdateAsync(string id, Train updatedTrain) =>
            await _trainsCollection.ReplaceOneAsync(x => x.id == id, updatedTrain);

        // Delete a train record from the database using its ID.
        public async Task DeleteAsync(string id) =>
            await _trainsCollection.DeleteOneAsync(x => x.id == id);
    }

}
