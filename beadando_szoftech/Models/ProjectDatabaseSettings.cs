namespace beadando_szoftech.Models
{
    public class ProjectDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UserCollectionName { get; set; } = null!;

        public string HouseCollectionName { get; set; } = null!;
    }
}
