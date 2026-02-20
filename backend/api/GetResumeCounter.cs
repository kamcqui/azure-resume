using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Company.Function
{
    public class Counter
    {
        public string id { get; set; } = "1";
        public int Count { get; set; }
    }

    public static class GetResumeCounter
    {
        [Function("GetResumeCounter")]
        public static HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            [CosmosDBInput("AzureResume", "Counter", Connection = "CosmosDbConnection", Id = "1", PartitionKey = "1")] Counter counter,
            [CosmosDBOutput("AzureResume", "Counter", Connection = "CosmosDbConnection")] out Counter updatedCounter)
        {
            updatedCounter = counter ?? new Counter { id = "1", Count = 0 };
            updatedCounter.Count++;

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            response.WriteAsJsonAsync(updatedCounter);
            return response;
        }
    }
}