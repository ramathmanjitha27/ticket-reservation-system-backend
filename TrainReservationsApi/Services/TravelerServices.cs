using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainReservationsApi.Models;

namespace TrainReservationsApi.Services
{
    public class TravelerServices
    {
        private readonly IMongoCollection<Traveler> _travelerColloction;

        public TravelerServices(IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            var mongoClient = new MongoClient(TrainReservationsDatabaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(TrainReservationsDatabaseSettings.Value.DatabaseName);
            _travelerColloction = mongoDb.GetCollection<Traveler>(TrainReservationsDatabaseSettings.Value.TravelersCollectionName);

        }

        //get all travelers
        public async Task<List<Traveler>> GetTravelersAsync() =>
            await _travelerColloction.Find(_ => true).ToListAsync();

        //get traveler by id
        public async Task<Traveler?> GetTravelerAsync(string id) =>
            await _travelerColloction.Find(x => x.Id == id).FirstOrDefaultAsync();

        //add new traveler
        public async Task CreateTravelerAsync(Traveler newTraveler)
        {
            // Hash the password
            newTraveler.Password = BCrypt.Net.BCrypt.HashPassword(newTraveler.Password);
            await _travelerColloction.InsertOneAsync(newTraveler);
        }


        //update traveler
        public async Task UpdateTravelerAsync(string id, Traveler updateTraveler) =>
             await _travelerColloction.ReplaceOneAsync(x => x.Id == id, updateTraveler);

        //delete traveler
        public async Task RemoveTravelerAsync(string id) =>
            await _travelerColloction.DeleteOneAsync(x => x.Id == id);

        public async Task<Traveler?> Login(string email, string password)
        {
            var traveler = await _travelerColloction.Find(t => t.Email == email).FirstOrDefaultAsync();

            // Check if the traveler exists and if the password is valid
            if (traveler != null && BCrypt.Net.BCrypt.Verify(password, traveler.Password))
            {
                return traveler;
            }

            return null;
        }
    }
}
