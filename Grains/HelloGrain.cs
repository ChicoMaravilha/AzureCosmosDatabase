using System;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.Azure.Documents.Client;

namespace Grains
{
    class HelloGrain : Orleans.Grain, IHello
    {
        private readonly Uri Endpoint = new Uri("https://<account>.azure.com:443/");
        private const string Key = "<key==>";

        public Task<string> CallDirect(string databaseName)
        {
            var client = new DocumentClient(Endpoint, Key);

            Console.WriteLine("Calling DocumentDB client");

            var database = client.CreateDatabaseQuery()
                    .Where(d => d.Id == databaseName)
                    .AsEnumerable()
                    .FirstOrDefault();

            Console.WriteLine("DocumentDB client returned");

            return Task.FromResult(database?.Id ?? $"Database {databaseName} doesn't exist");
        }

        public async Task<string> CallWrapped(string databaseName)
        {
            var client = new DocumentClient(Endpoint, Key);

            Console.WriteLine("Calling DocumentDB client");

            var database = await Task.Run(() => client.CreateDatabaseQuery()
                    .Where(d => d.Id == databaseName)
                    .AsEnumerable()
                    .FirstOrDefault()).ConfigureAwait(false);

            Console.WriteLine("DocumentDB client returned");

            return database?.Id ?? $"Database {databaseName} doesn't exist";
        }
    }
}
