using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Models.Error
{
    public class RootError
    {
        [JsonProperty("error")]
        public Error error { get; set; }
    }

    public class Error
    {
        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }
}
