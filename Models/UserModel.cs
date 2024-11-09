using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
[BsonIgnoreExtraElements]

public class UserModel
{            
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("fullname")]
    public string? Fullname { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("password")]
    public string? Password { get; set; }
    
    [BsonElement("image")]
    public string? Image64 { get; set; }

    [BsonElement("searches")]
    public List<string> SearchHistory { get; set; } = new List<string>(); 

}
