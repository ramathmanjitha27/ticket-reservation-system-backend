using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainReservationsApi.Models;

namespace TrainReservationsApi.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<Staff> _staffCollection;

        public AuthService(IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
               TrainReservationsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                TrainReservationsDatabaseSettings.Value.DatabaseName);

            _staffCollection = mongoDatabase.GetCollection<Staff>(
                TrainReservationsDatabaseSettings.Value.StaffCollectionName);
        }

        public async Task<Staff> StaffLogin(StaffLogin login)
        {
            return await _staffCollection.Find(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefaultAsync();
        }

    }
}
