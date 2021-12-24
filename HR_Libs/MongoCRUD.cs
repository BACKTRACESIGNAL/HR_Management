using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HR_Management.HR_Libs
{
    public class MongoCRUD
    {
        private IMongoDatabase m_db;
        public MongoCRUD(IMongoDatabase db)
        {
            this.m_db = db;
        }

        public void InsertOne(String collection, String document)
        {
            var doc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(document);
            var coll = this.m_db.GetCollection<MongoDB.Bson.BsonDocument>(collection);
            coll.InsertOne(doc);
        }

        public List<T> GetMany<T>(string collection, string filter)
        {
            var filter_json = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(filter);
            var coll = this.m_db.GetCollection<T>(collection);
            return coll.Find(filter_json).ToList();
        }
    }
}
