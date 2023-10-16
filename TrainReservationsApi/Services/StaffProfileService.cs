using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainReservationsApi.Models;

namespace TrainReservationsApi.Services
{
    public class StaffProfileService
    {
        private readonly IMongoCollection<Staff> _staffCollection;

        // Constructor for StaffProfileService that receives database configuration settings.
        public StaffProfileService(IOptions<TrainReservationsDatabaseSettings> TrainReservationsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
               TrainReservationsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                TrainReservationsDatabaseSettings.Value.DatabaseName);

            // Initialize the MongoDB collection for staff profiles.
            _staffCollection = mongoDatabase.GetCollection<Staff>(
                TrainReservationsDatabaseSettings.Value.StaffCollectionName);
        }

        // Retrieve a list of all staff members from the MongoDB collection.
        public async Task<List<Staff>> GetAllStaffMembers() =>
            await _staffCollection.Find(_ => true).ToListAsync();

        // Retrieve a staff member by their unique identifier (id) from the MongoDB collection.
        public async Task<Staff> GetStaffMemberById(string id)
        {
            return await _staffCollection.Find<Staff>(staff => staff.Id == id).FirstOrDefaultAsync();
        }

        // Create a new staff member profile in the MongoDB collection.
        public async Task CreateStaffMember(Staff newStaffMemeber)
        {
            await _staffCollection.InsertOneAsync(newStaffMemeber);
        }

        // Update an existing staff member's profile in the MongoDB collection.
        public async Task UpdateStaffMember(string id, Staff updateStaff)
        {
            await _staffCollection.ReplaceOneAsync(x => x.Id == id, updateStaff);
        }

        // Remove a staff member's profile from the MongoDB collection based on their unique identifier.
        public async Task RemoveStaffMember(string id) =>
            await _staffCollection.DeleteOneAsync(x => x.Id == id);


        public async Task<Staff?> LoginStaff(string email, string password)
        {
            var staff = await _staffCollection.Find(t => t.Email == email).FirstOrDefaultAsync();

            // Check if the traveler exists and if the password is valid
            //if (staff != null && BCrypt.Net.BCrypt.Verify(password, staff.Password))
            //{
            //    return staff;
            //}

            if(staff != null && staff.Password == password){
                return staff;
            }

            return null;
        }
    }
}
