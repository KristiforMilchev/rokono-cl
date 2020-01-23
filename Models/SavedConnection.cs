namespace RokonoDbManager.Models
{
    public class SavedConnection
    {
        public string ConnectionString { get; set; }
        public string  Host { get; set; }
        public string FilePath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int ConnectionId { get; set; }
        public string DbContextPath {get; set;}
        
    }
}