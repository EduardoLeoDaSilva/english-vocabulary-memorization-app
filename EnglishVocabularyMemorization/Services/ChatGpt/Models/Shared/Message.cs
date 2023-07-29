using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Models.Shared
{
    public class Message
    {
        /// <summary>
        /// The role of the author of this message. One of "system", "user", or "assistant".
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }
        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
        /// <summary>
        /// The name of the author of this message. May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        public string Name { get; set; }
    }
}
