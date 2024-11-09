using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace teeze.Models
{
    [BsonIgnoreExtraElements]
    public class ProductModel   
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id  { get; set; }

        [BsonElement("name")] 
        public string? Name { get; set; }

        [BsonElement("price")] 
        public double Price { get; set; }

        [BsonElement("thumbnail")] 
        public string? Thumbnail { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }
        
        [BsonElement("id")]
        public string? Id_ { get; set; }

        [BsonElement("images")]
        public List<string>? Images { get; set; }

    }
}
