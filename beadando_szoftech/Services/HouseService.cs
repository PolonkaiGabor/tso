using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace beadando_szoftech.Services
{
    public class HouseService
    {
        private readonly IMongoCollection<House> _houseCollection;
        private readonly IMongoCollection<Counter> _counterCollection;

        public HouseService(IOptions<ProjectDatabaseSettings> ProjectDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ProjectDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ProjectDatabaseSettings.Value.DatabaseName);

            _houseCollection = mongoDatabase.GetCollection<House>(
                ProjectDatabaseSettings.Value.HouseCollectionName);

            _counterCollection = mongoDatabase.GetCollection<Counter>("counters");

        }

        private async Task<int> GetNextSequenceValue(string sequenceName)
        {
            var filter = Builders<Counter>.Filter.Eq(c => c.Id, sequenceName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var counter = await _counterCollection.FindOneAndUpdateAsync(filter, update, options);
            return counter.SequenceValue;
        }

        //Kilistázza az összes házat
        public async Task<List<House>> GetAsync() =>
            await _houseCollection.Find(_ => true).ToListAsync();

        //visszaadja a házat id alapján
        public async Task<House?> GetAsync(int id) =>
            await _houseCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        //Új ház létrehozása az adatbázisban
        public async Task CreateAsync(House newHouse)
        {
            newHouse.id = await GetNextSequenceValue("houseId");
            await _houseCollection.InsertOneAsync(newHouse);
        }

        //A ház adatainak módosítása az adatbázisban id alapján
        public async Task UpdateAsync(int id, House updatedHouse) =>
            await _houseCollection.ReplaceOneAsync(x => x.id == id, updatedHouse);

        //A ház törlése id alapján
        public async Task RemoveAsync(int id) =>
            await _houseCollection.DeleteOneAsync(x => x.id == id);
    }
}
