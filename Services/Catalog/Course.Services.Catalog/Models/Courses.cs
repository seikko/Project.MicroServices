using Course.Services.Catalog.Dtos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Models
{
    public class Courses
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Picture { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }

        //Relational Properties
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public Feature Feature { get; set; }
        [BsonIgnore] // mongo dbdeki collection satır olarak yansıtma.
        public Category Categories{ get; set; }

    }
}
