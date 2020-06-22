using Newtonsoft.Json;

namespace SampleAPITestAutomationProjectInDotnetCore
{

    public class NewlyCreatedEmployee
    {
            [JsonProperty("id")]
            public int id { get; set; }

             [JsonProperty("name")]
            public string? name { get; set; }

             [JsonProperty("salary")]
            public string? salary { get; set; }

             [JsonProperty("age")]
            public string? age { get; set; }



        
    }
}
