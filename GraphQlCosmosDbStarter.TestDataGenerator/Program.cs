using System.Net;
using AutoFixture;
using GraphQlCosmosDbStarter.Data.Models;
using Microsoft.Azure.Cosmos;

namespace GraphQlCosmosDbStarter.TestDataGenerator;

internal class Program
{
    private static readonly string CosmosDbConnectionString = "";

    private Container _container;

    private CosmosClient _cosmosClient;
    private Database _database;

    //The name of the container we will create
    private readonly string containerId = "sites";

    // The name of the database and container we will create
    private readonly string databaseId = "demo-db";

    // <Main>
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Beginning data generation");
            var p = new Program();
            await p.GetStartedDemoAsync();
        }
        catch (CosmosException de)
        {
            var baseException = de.GetBaseException();
            Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        finally
        {
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
    // </Main>

    // <GetStartedDemoAsync>
    /// <summary>
    ///     Entry point to call methods that operate on Azure Cosmos DB resources in this sample
    /// </summary>
    public async Task GetStartedDemoAsync()
    {
        // Create a new instance of the Cosmos Client
        _cosmosClient = new CosmosClient(CosmosDbConnectionString, new CosmosClientOptions
        {
            ApplicationName = "GraphQlCosmosDbStarter.TestDataGenerator",
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        });

        await CreateDatabaseAsync();
        await CreateContainerAsync();
        await ScaleContainerAsync();
        await AddSitesToContainerAsync(100);
        //await DeleteDatabase(); //Only uncomment if you want the test db deleted
    }

    private async Task CreateDatabaseAsync()
    {
        // Create a new database
        _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Console.WriteLine("Created Database: {0}\n", _database.Id);
    }

    private async Task CreateContainerAsync()
    {
        // Create a new container
        _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
        Console.WriteLine("Created Container: {0}\n", _container.Id);
    }

    private async Task ScaleContainerAsync()
    {
        // Read the current throughput
        try
        {
            var throughput = await _container.ReadThroughputAsync();
            if (throughput.HasValue)
            {
                Console.WriteLine("Current provisioned throughput : {0}\n", throughput.Value);
                var newThroughput = throughput.Value + 100;
                // Update throughput
                await _container.ReplaceThroughputAsync(newThroughput);
                Console.WriteLine("New provisioned throughput : {0}\n", newThroughput);
            }
        }
        catch (CosmosException cosmosException) when (cosmosException.StatusCode == HttpStatusCode.BadRequest)
        {
            Console.WriteLine("Cannot read container throuthput.");
            Console.WriteLine(cosmosException.ResponseBody);
        }
    }

    private async Task AddSitesToContainerAsync(int numberOfItemsToAdd)
    {
        var autoFixture = new Fixture();
        autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => autoFixture.Behaviors.Remove(b));
        autoFixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

        var itemsToAdd = autoFixture.CreateMany<Site>(numberOfItemsToAdd).ToList();

        for (var i = 0; i < itemsToAdd.Count(); i++)
        {
            var site = itemsToAdd[i];

            try
            {
                // Read the item to see if it exists.  
                var response =
                    await _container.ReadItemAsync<Site>($"{site.Id}.{i}", new PartitionKey(site.PartitionKey));
                Console.WriteLine("Item in database with id: {0} already exists\n", response.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                var response = await _container.CreateItemAsync(site, new PartitionKey(site.PartitionKey));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n",
                    response.Resource.Id, response.RequestCharge);
            }
        }
    }

    private async Task DeleteDatabase() => await _database.DeleteAsync();
}
