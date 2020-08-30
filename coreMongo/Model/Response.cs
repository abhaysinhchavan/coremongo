using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace coreMongo.Model
{
    [DataContract]
    public class Response
    {
        [JsonPropertyName("data")]
        [DataMember(Name = "data")]
        public dynamic Data { get; set; }

        [JsonPropertyName("error")]
        [DataMember(Name = "error")]
        public ErrorResponse Error { get; set; }
    }
}
