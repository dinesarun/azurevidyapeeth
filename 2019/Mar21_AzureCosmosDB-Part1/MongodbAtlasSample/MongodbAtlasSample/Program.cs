using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbAtlasSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Azure Cosmos DB connection string
            string connectionString = "mongodb://7fdb806e-0ee0-4-231-b9ee:9tBemRpqEFHcADgKXmKsapxJX0ZohIxPwz3NG7RJCrN0WKi0s8PaF5Efz0glxDdCYqv5M0wSjLb6NYq9lbhibA==@7fdb806e-0ee0-4-231-b9ee.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

            // MongoDB database connection string
            //string connectionString = "mongodb://207.246.126.73:27017";

            string databaseName = "azuredemo";
            string collectionName = "myCollection";

            var client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            server.Connect();

            var databaseList = server.GetDatabaseNames().ToList();

            MongoDatabase database = server.GetDatabase(databaseName);
            var collections = database.GetCollectionNames().ToList();
            if (!collections.Contains(collectionName))
            {
                database.CreateCollection(collectionName);
            }

            // Prepare data
            var sampleData = new Dictionary<string, object>();
            sampleData.Add("Id", "ALFKI");
            sampleData.Add("CompanyName", "Microsoft");
            sampleData.Add("ContactName", "Maria Anders");
            sampleData.Add("ContactTitle", "Sales Representative");
            sampleData.Add("Address", "Obere Str. 57");
            sampleData.Add("City", "Berlin");
            sampleData.Add("PostalCode", "12209");
            sampleData.Add("Country", "Germany");
            sampleData.Add("Phone", "030-0074321");
            sampleData.Add("Fax", "030-0076545");

            // Insert data
            //database.GetCollection(collectionName).Insert(new BsonDocument(sampleData));

            // Read data
            IMongoDatabase database1 = client.GetDatabase(databaseName);
            var collection = database1.GetCollection<BsonDocument>(collectionName);
            collection.Find(new BsonDocument()).ForEachAsync(X => Console.WriteLine(X));

            Console.WriteLine("Demo completed");
            Console.ReadLine();
            //GetAtlasData();
        }

        #region Workout

        static void GetAtlasData()
        {
            //MongoClient client = new MongoClient(new MongoClientSettings()
            //{
            //    Server = new MongoServerAddress("207.246.126.73", 27017)
            //});

            MongoClient client = new MongoClient("mongodb://207.246.126.73:27017");
            MongoServer server = client.GetServer();
            server.Connect();
            var databaseList = server.GetDatabaseNames().ToList();

            //db creation

            MongoDatabase database = server.GetDatabase("test1");

            //create collection

            //database.CreateCollection("myCollection");

            //Insert collection

            Dictionary<string, object> dataValue = new Dictionary<string, object>();
            dataValue.Add("Id", "ALFKI");
            dataValue.Add("CompanyName", "Alfreds Futterkiste");
            dataValue.Add("ContactName", "Maria Anders");
            dataValue.Add("ContactTitle", "Sales Representative");
            dataValue.Add("Address", "Obere Str. 57");
            dataValue.Add("City", "Berlin");
            dataValue.Add("PostalCode", "12209");
            dataValue.Add("Country", "Germany");
            dataValue.Add("Phone", "030-0074321");
            dataValue.Add("Fax", "030-0076545");

            //database.GetCollection("myCollection").Insert(new BsonDocument(dataValue));

            IMongoDatabase database1 = client.GetDatabase("test1");
            var collection = database1.GetCollection<BsonDocument>("myCollection");
            collection.Find(new BsonDocument()).ForEachAsync(X => Console.WriteLine(X));
            var collectionList = database1.ListCollections().ToListAsync().Result.Select(a => a.First()).Select(b => b.Value.ToString()).ToList();
            var cursor = collection.Find(FilterDefinition<BsonDocument>.Empty).ToCursorAsync();
            cursor.Result.MoveNext();
            List<BsonDocument> resultSet = cursor.Result.Current.ToList();

            ///// Filter operations.

            //IMongoCollection<BsonDocument> myCollection  = database.GetCollection<BsonDocument>("mycollection");
            //AggregateOptions aggregateOptions = new AggregateOptions();
            //aggregateOptions.BatchSize = 10000;
            //IAggregateFluent<BsonDocument> aggregateCollection = myCollection.Aggregate(aggregateOptions);
            //var filterCursor = aggregateCollection.Match(Builders<BsonDocument>.Filter.Eq("Age", 25)).ToCursorAsync();
            //filterCursor.Result.MoveNext();
            //List<BsonDocument> queryResult = filterCursor.Result.Current.ToList();

            //Builders<BsonDocument>.Filter.Eq("Age", 25)
            //Builders<BsonDocument>.Filter.Ne("Age", 25)
            //Builders<BsonDocument>.Filter.Lt("Age", 25)
            //Builders<BsonDocument>.Filter.Lte("Age", 25)
            //Builders<BsonDocument>.Filter.Gt("Age", 25)
            //Builders<BsonDocument>.Filter.Gte("Age", 25)
            //Builders<BsonDocument>.Filter.In("Age", 25)

            Console.WriteLine("Data printed");
            Console.ReadLine();
        }

        #endregion
    }
}
