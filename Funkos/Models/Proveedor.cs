using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkos.Models;

public record Proveedor
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("name")]
    public required string Name { get; set; }
    
    [BsonElement("descripcion")]
    public required string Descripcion  { get; set; }
        
    [BsonElement("createAt")]
    public DateTime CreateAt {get; set; }
    
    [BsonElement("updateAt")]
    public DateTime? UpdateAt {get; set; } 
    
    [BsonElement("isDeleted")]
    public bool IsDeleted {get; set; }
    
    [BsonElement("deletaAt")]
    public DateTime? DeleteAt {get; set; }
}