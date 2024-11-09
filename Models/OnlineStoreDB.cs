namespace teeze.Models
{
    public class OnlineStoreDB : IOnlineStoreDB
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
