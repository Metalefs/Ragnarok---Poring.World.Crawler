using System.Diagnostics;
//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson;

namespace Domain
{
    [DebuggerDisplay("{Model},{Price}")]
    public class Item
    {
        //[BsonId()]
        //public ObjectId Id { get; set; }
        //[BsonRequired()]
        public string Model { get; set; }
        //[BsonRequired()]
        public float Price { get; set; }
        //[BsonRequired()]
        public string Link { get; set; }
        //[BsonRequired()]
        public string ImageURL { get; set; }

        public string Percent{ get; set; }
        public string Stock  { get; set; }
        public string Buyers { get; set; }
        public string IsSnap { get; set; }
        public string SnapEnd{ get; set; }
    }
}
