using ExchangeRates.Config;
using ExchangeRates.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Repositories.Implementation
{
    public class ApiRequestsRepository : IApiRequestsRepository
    {
        protected static IMongoClient _mongoClient;
        private readonly MyConfig _config;

        public ApiRequestsRepository(MyConfig config)
        {
            _config = config;
        }

        public async Task SaveApiRequestToDatabase(HttpRequestMessage requestMessage)
        {
            _mongoClient = new MongoClient(_config.ConnectionString);
            var database = _mongoClient.GetDatabase("ExchangeRatesDb");
            var collection = database.GetCollection<BsonDocument>("ApiRequests");

            await collection.InsertOneAsync(requestMessage.ToBsonDocument());
        }
    }
}
