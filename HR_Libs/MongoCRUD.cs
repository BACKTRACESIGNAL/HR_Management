using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void InsertOne(COLLECTION collection, BsonDocument document)
        {
            var coll = this.m_db.GetCollection<BsonDocument>(collection.ToDescriptionString());
            coll.InsertOne(document);
        }

        public void UpdateOne(COLLECTION collection, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> document)
        {
            var coll = this.m_db.GetCollection<BsonDocument>(collection.ToDescriptionString());
            coll.UpdateOne(filter, document);
        }

        public void UpdateOne(COLLECTION collection, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> document, List<ArrayFilterDefinition> definitions)
        {
            var coll = this.m_db.GetCollection<BsonDocument>(collection.ToDescriptionString());
            var updateOptions = new UpdateOptions { ArrayFilters = definitions };
            coll.UpdateOne(filter, document, updateOptions);
        }

        public List<T> GetMany<T>(COLLECTION collection, FilterDefinition<T> filter)
        {
            var coll = this.m_db.GetCollection<T>(collection.ToDescriptionString());
            return coll.Find(filter).ToList<T>();
        }

        public List<T> GetMany<T>(COLLECTION collection, FilterDefinition<BsonDocument> filter, ProjectionDefinition<BsonDocument> projection)
        {
            var coll = this.m_db.GetCollection<BsonDocument>(collection.ToDescriptionString());
            return coll.Find<BsonDocument>(filter).Project<T>(projection).ToList<T>();
        }

        public List<T> GetDistinct<T>(COLLECTION collection, String field, FilterDefinition<BsonDocument> filter)
        {
            var coll = this.m_db.GetCollection<BsonDocument>(collection.ToDescriptionString());
            return coll.Distinct<T>(field, filter).ToList<T>();
        }

        public BulkWriteResult<BsonDocument> BulkWrite(COLLECTION collection, List<WriteModel<BsonDocument>> listWrites)
        {
            var coll = this.m_db.GetCollection<BsonDocument>(collection.ToDescriptionString());
            return coll.BulkWrite(listWrites);
        }
    }
}
