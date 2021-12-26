using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static HR_Management.HR_Libs.MongoDefine;

namespace HR_Management.HR_Libs
{
    public class MongoCRUD
    {
        private IMongoDatabase m_db;
        public MongoCRUD(IMongoDatabase db)
        {
            this.m_db = db;
        }

        public void InsertOne(COLLECTION collection, String document)
        {
            var doc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(document);
            var coll = this.m_db.GetCollection<MongoDB.Bson.BsonDocument>(collection.ToDescriptionString());
            coll.InsertOne(doc);
        }

        public List<T> GetMany<T>(COLLECTION collection, string filter)
        {
            var filter_json = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(filter);
            var coll = this.m_db.GetCollection<T>(collection.ToDescriptionString());
            return coll.Find(filter_json).ToList();
        }
    }
}
