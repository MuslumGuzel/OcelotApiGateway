using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiGateway.Models;

public class Logs
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }    
    public string Request { get; set; } = null!;
    public string Response { get; set; } = null!;
    public DateTime CreateDate { get; set; }
}