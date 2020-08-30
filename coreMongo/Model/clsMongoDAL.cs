using coreMongo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace coreMongo.Model
{
    public class clsMongoDAL : IMongo
    {
        protected static IMongoClient objMdbClient;
        protected static IMongoDatabase objMongoDB;
        protected IMongoCollection<BsonDocument> mongoCollection;
        protected string strMongoDBConnectionstring;
        protected string strMongoDataBase;
        public List<BsonDocument> lstDocuments = null;
      
        public clsMongoDAL(string strDataBase)
        {
            strMongoDBConnectionstring = "mongodb://localhost:27017";
            objMdbClient = new MongoClient(strMongoDBConnectionstring);
            objMongoDB = objMdbClient.GetDatabase(strDataBase);
        }
      

        public IEnumerable<dynamic> GetAll(string strCollectionName)
        {
            try
            {
                mongoCollection = objMongoDB.GetCollection<BsonDocument>(strCollectionName);
                var collections = mongoCollection.Find(new BsonDocument());
                lstDocuments = collections.ToList<BsonDocument>();
            }
            catch (Exception ex) { }
            return (IEnumerable<dynamic>)lstDocuments;
        }
        public IEnumerable<dynamic> GetAllMatching(string strCollectionName, string strField, object strValue)
        {
            var varFilter = Builders<BsonDocument>.Filter.Eq(strField, strValue);
            try
            {
                mongoCollection = objMongoDB.GetCollection<BsonDocument>(strCollectionName);
                var results = mongoCollection.Find(varFilter).ToCursor();
                lstDocuments = results.ToList<BsonDocument>();
            }
            catch (Exception ex) { }
            return (IEnumerable<dynamic>)lstDocuments;
        }
        public IEnumerable<dynamic> GetWithPagination(string strCollectionName, string strField, object strValue, int pageSize, int pageNum)
        {
            var varFilter = Builders<BsonDocument>.Filter.Eq(strField, strValue);
            try
            {
                mongoCollection = objMongoDB.GetCollection<BsonDocument>(strCollectionName);
                var results = mongoCollection.Find(varFilter).Skip((pageSize * (pageNum-1))).Limit(pageSize);
                lstDocuments = results.ToList<BsonDocument>();
            }
            catch (Exception ex) { }
            return (IEnumerable<dynamic>)lstDocuments;
        }
       
        public long GetCount(string strCollectionName, string strField, object strValue)
        {
            var varFilter = Builders<BsonDocument>.Filter.Eq(strField, strValue);
            long lngResult = 0;
            try
            {
                mongoCollection = objMongoDB.GetCollection<BsonDocument>(strCollectionName);
                var results = mongoCollection.Find(varFilter).CountDocuments();
                lngResult = Convert.ToInt64(results); 
            }
            catch (Exception ex) { }
            return lngResult;
        }

        public async Task<List<BsonDocument>> GetAllMachingAsync(string strCollectionName, string strField, string strValue)
        {
            var varFilter = Builders<BsonDocument>.Filter.Eq(strField, strValue);
            try
            {
                mongoCollection = objMongoDB.GetCollection<BsonDocument>(strCollectionName);
                var results = await mongoCollection.Find(varFilter).ToCursorAsync();
                lstDocuments = results.ToList<BsonDocument>();
            }
            catch (Exception ex) { }
            return lstDocuments;
        }

        public async Task<List<BsonDocument>> GetAllMachingMultiFields(string strCollectionName, FilterDefinition<BsonDocument> filterDefinitions)
        {
            //var varFilter = Builders<BsonDocument>.Filter.And(arrfilterDefinitions);
            try
            {
                mongoCollection = objMongoDB.GetCollection<BsonDocument>(strCollectionName);
                var results = await mongoCollection.Find(filterDefinitions).ToCursorAsync();
                lstDocuments = results.ToList<BsonDocument>();
            }
            catch (Exception ex) { }
            return lstDocuments;
        }

        public bool blnInsertDocument(string strCollectionName, BsonDocument bsonDocument)
        {
            bool blnReturn = false;
            try
            {
                if ((!string.IsNullOrEmpty(strCollectionName)) && (bsonDocument != null))
                {
                    objMongoDB.GetCollection<BsonDocument>(strCollectionName).WithWriteConcern(WriteConcern.Acknowledged).InsertOne(bsonDocument);
                    blnReturn = true;
                }
            }
            catch (Exception ex)
            {
                blnReturn = false;
            }
            return blnReturn;
        }

        public bool blnUpdateDocument(string strCollectionName, FilterDefinition<BsonDocument> varFilter,
           UpdateDefinition<BsonDocument> updateDefinition)
        {
            bool blnReturn = false;
            try
            {
                var result = objMongoDB.GetCollection<BsonDocument>(strCollectionName).FindOneAndUpdate(varFilter, updateDefinition);
                //.UpdateOne(varFilter, updateDefinition);
                if (result != null)
                {
                    blnReturn = true;
                }
            }
            catch (Exception ex)
            {
                blnReturn = false;
            }
            return blnReturn;
        }

        public bool blnDeleteDocument(string strCollectionName, string strField, object strValue)
        {
            bool blnReturn = false;
            var varFilter = Builders<BsonDocument>.Filter.Eq(strField, strValue);
            try
            {
                var Deleteone = objMongoDB.GetCollection<BsonDocument>(strCollectionName).DeleteOneAsync(varFilter);
                if (Convert.ToInt32(Deleteone.Result.DeletedCount) > 0)
                { blnReturn = true; }
            }
            catch (Exception ex) { }
            return blnReturn;
        }

        public bool blnCheckDocument(string strCollectionName, string strField, object strValue)
        {
            bool blnReturn = false;
            var varFilter = Builders<BsonDocument>.Filter.Eq(strField, strValue);
            try
            {
                var check = objMongoDB.GetCollection<BsonDocument>(strCollectionName).FindAsync(varFilter);
                if (check.Result.Any())
                { blnReturn = true; }
            }
            catch (Exception ex) { }
            return blnReturn;
        }

        
    }
}
