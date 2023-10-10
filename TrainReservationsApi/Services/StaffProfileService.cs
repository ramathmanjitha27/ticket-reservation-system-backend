using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainReservationsApi.Models;

namespace TrainReservationsApi.Services
{
    public class StaffProfileService
    {
        private readonly IMongoCollection<Staff> _staffCollection;

        public StaffProfileService(IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
               TrainReservationsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                TrainReservationsDatabaseSettings.Value.DatabaseName);

            _staffCollection = mongoDatabase.GetCollection<Staff>(
                TrainReservationsDatabaseSettings.Value.StaffCollectionName);
        }

        public async Task<List<Staff>> GetAllStaffMembers() =>
            await _staffCollection.Find(_ => true).ToListAsync();

        public async Task<Staff> GetStaffMemberById(string id)
        {
            return await _staffCollection.Find<Staff>(staff => staff.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateStaffMember(Staff newStaffMemeber)
        {
            await _staffCollection.InsertOneAsync(newStaffMemeber);
        }

        public async Task UpdateStaffMember(string id, Staff updateStaff)
        {
            Console.WriteLine(id);
            Console.WriteLine(updateStaff);
            await _staffCollection.ReplaceOneAsync(x => x.Id == id, updateStaff);
        }

        public async Task RemoveStaffMember(string id) =>
            await _staffCollection.DeleteOneAsync(x => x.Id == id);
    }
}
