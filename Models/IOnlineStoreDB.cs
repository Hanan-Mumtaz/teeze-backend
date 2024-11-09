namespace teeze.Models
{
    public interface IOnlineStoreDB
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
