using beadando_szoftech.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace beadando_szoftech.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IOptions<ProjectDatabaseSettings> ProjectDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ProjectDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ProjectDatabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                ProjectDatabaseSettings.Value.UserCollectionName);

        }

        //Kilistázza az összes felhasználót
        public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        //visszaadja a felhasználót id alapján
        public async Task<User?> GetAsync(string id) =>
            await _userCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        //visszaadja a felhasználót email alapján
        public async Task<User?> GetBasedOnEmailAsync(string email) =>
            await _userCollection.Find(x => x.email == email).FirstOrDefaultAsync();

        //visszaadja a felhasználót username alapján
        public async Task<User?> GetBasedOnUsernameAsync(string username) =>
            await _userCollection.Find(x => x.username == username).FirstOrDefaultAsync();

        //Új felhasználó létrehozása az adatbázisban
        public async Task CreateAsync(User newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        //A felhasználó valamelyk adatának módosítása az adatbázisban
        public async Task UpdateAsync(string id, User updatedUser) =>
            await _userCollection.ReplaceOneAsync(x => x.id == id, updatedUser);

        //Felhasználó törlése id alapján
        public async Task RemoveAsync(string id) =>
            await _userCollection.DeleteOneAsync(x => x.id == id);

    }
}
