using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

namespace Api
{
    public class GetResumeCounter
    {
        private readonly ILogger _logger;
        private readonly Container _container;

        public GetResumeCounter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetResumeCounter>();

            string connectionString = Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING");

            var client = new CosmosClient(connectionString);
            _container = client.GetContainer("cloudresume", "counter");
        }

        [Function("GetResumeCounter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Processing resume counter request.");

            // Read item
            var response = await _container.ReadItemAsync<Counter>("1", new PartitionKey("1"));
            var counter = response.Resource;

            // Increment
            counter.count += 1;

            // Save updated value
            await _container.ReplaceItemAsync(counter, "1", new PartitionKey("1"));

            // Return JSON
            var httpResponse = req.CreateResponse(HttpStatusCode.OK);
            await httpResponse.WriteAsJsonAsync(counter);

            return httpResponse;
        }
    }

    public class Counter
    {
        public string id { get; set; }
        public int count { get; set; }
    }
}
