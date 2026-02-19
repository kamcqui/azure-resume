using Newtonsoft.Json;

public class Counter
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }
}