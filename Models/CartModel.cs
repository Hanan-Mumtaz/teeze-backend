using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace teeze.Models
{
    [BsonIgnoreExtraElements]
    public class CartModel     
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id  { get; set; }

        [BsonElement("id")]
        public string? Id_ { get; set; }
       
        [BsonElement("name")] 
        public string? Name { get; set; }

        [BsonElement("price")] 
        public double Price { get; set; }

        [BsonElement("thumbnail")] 
        public string? ThumbnaiL { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }
        
        [BsonElement("quantity")]
        public int Quantity { get; set;}
    }
}
