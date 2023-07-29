using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.Services.Wordup.Models
{
    public class UserWords
    {
        public UserWords()
        {

        }
        [JsonProperty]
        public string UnknownWords { get; set; }
        [JsonProperty]
        public string KnownWords { get; set; }
    }
}
