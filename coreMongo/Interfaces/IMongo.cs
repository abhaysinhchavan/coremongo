using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace coreMongo.Interfaces
{
    interface IMongo
    {
        IEnumerable<dynamic> GetAll(string strCollectionName);
        IEnumerable<dynamic> GetAllMatching(string strCollectionName, string strField, object strValue);
        Task<List<BsonDocument>> GetAllMachingAsync(string strCollectionName, string strField, string strValue);
        public IEnumerable<dynamic> GetWithPagination(string strCollectionName, string strField, object strValue, int pageSize, int pageNum);
        bool blnInsertDocument(string strCollectionName, BsonDocument bsonDocument);
        bool blnUpdateDocument(string strCollectionName, FilterDefinition<BsonDocument> varFilter,
           UpdateDefinition<BsonDocument> updateDefinition);
        bool blnDeleteDocument(string strCollectionName, string strField, object strValue);

        public long GetCount(string strCollectionName, string strField, object strValue);
        bool blnCheckDocument(string strCollectionName, string strField, object strValue);
    }
}
