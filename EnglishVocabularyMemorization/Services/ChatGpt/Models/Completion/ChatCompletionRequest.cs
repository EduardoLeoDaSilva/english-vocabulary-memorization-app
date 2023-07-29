using EnglishVocabularyMemorization.Services.ChatGpt.Models.Shared;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Models.Completion
{
    public class ChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }
        /// <summary>
        /// A list of messages describing the conversation so far.
        /// </summary>

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }
}
