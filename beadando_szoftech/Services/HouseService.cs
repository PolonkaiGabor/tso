using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace beadando_szoftech.Services
{
    public class HouseService
    {
        private readonly IMongoCollection<House> _houseCollection;

        public HouseService(IOptions<ProjectDatabaseSettings> ProjectDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ProjectDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ProjectDatabaseSettings.Value.DatabaseName);

            _houseCollection = mongoDatabase.GetCollection<House>(
                ProjectDatabaseSettings.Value.HouseCollectionName);

        }

        //Kilistázza az összes házat
        public async Task<List<House>> GetAsync() =>
            await _houseCollection.Find(_ => true).ToListAsync();

        //visszaadja a házat id alapján
        public async Task<House?> GetAsync(string id) =>
            await _houseCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        //Új ház létrehozása az adatbázisban
        public async Task CreateAsync(House newHouse) =>
            await _houseCollection.InsertOneAsync(newHouse);

        //A ház adatainak módosítása az adatbázisban id alapján
        public async Task UpdateAsync(string id, House updatedHouse) =>
            await _houseCollection.ReplaceOneAsync(x => x.id == id, updatedHouse);

        //A ház törlése id alapján
        public async Task RemoveAsync(string id) =>
            await _houseCollection.DeleteOneAsync(x => x.id == id);
    }
}
