using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureCosmosDbStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var endpoint = ConfigurationManager.AppSettings["DocDbEndpoint"];
                var masterKey = ConfigurationManager.AppSettings["DocDbMasterKey"];
                var databaseName = "ToDoList";
                var collectionName = "Items";
                var storedProcedureName = "spHelloWorld";

                using (var client = new DocumentClient(new Uri(endpoint), masterKey))
                {
                    Console.WriteLine("\r\n>>>>>>>>>>>>>>>> Creating Database <<<<<<<<<<<<<<<<<<<");
                    // Create new database Object
                    //Id defines name of the database
                    var dbDefinition = new Database { Id = databaseName };
                    var db = await client.CreateDatabaseIfNotExistsAsync(dbDefinition);
                    Console.WriteLine($"Database {databaseName} created successfully");

                    //Create new database collection
                    Console.WriteLine("\r\n>>>>>>>>>>>>>>>> Creating Collection <<<<<<<<<<<<<<<<<<<");
                    var collectionDefinition = new DocumentCollection { Id = collectionName };
                    var collection = await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName),
                        collectionDefinition);
                    Console.WriteLine($"Collection {collectionName} created successfully");

                    var sprocBody = File.ReadAllText(@"..\..\StoredProcedures\spHelloWorld.js");
                    var spDefinition = new StoredProcedure
                    {
                        Id = storedProcedureName,
                        Body = sprocBody
                    };


                    ////Execute Store Procedure
                    Item newItem = new Item
                    {
                        Category = "personal",
                        Name = "Groceries",
                        Description = "Pick up blueberries, red berries",
                        Completed = true,
                    };
                    var result = await client.ExecuteStoredProcedureAsync<string>
                        (UriFactory.CreateStoredProcedureUri(databaseName, collectionName, "createToDoItem"), new RequestOptions { PartitionKey = new PartitionKey(newItem.Category) }, newItem);
                    Console.WriteLine($"Executed Store Procedure: response:{result.Response}");


                    ////Create Store Procedure
                    //StoredProcedure sproc = await client.CreateStoredProcedureAsync
                    //    (UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), spDefinition);
                    //Console.WriteLine($"\r\nCreated Store procedure Id:{sproc.Id} ");

                    ////Execute Store Procedure
                    //var result = await client.ExecuteStoredProcedureAsync<string>
                    //    (UriFactory.CreateStoredProcedureUri(databaseName, collectionName, storedProcedureName), new RequestOptions { PartitionKey = new PartitionKey("test") });
                    //Console.WriteLine($"Executed Store Procedure: response:{result.Response}");

                    ////Delete Store Procedure
                    //await client.DeleteStoredProcedureAsync
                    //    (UriFactory.CreateStoredProcedureUri(databaseName, collectionName, storedProcedureName));
                    //Console.WriteLine("Stored Procedure Deleted Successfully");

                    Console.ReadKey();
                }
            }).Wait();
        }
    }

    public class Item
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}
