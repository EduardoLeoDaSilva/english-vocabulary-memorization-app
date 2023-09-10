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
        [JsonProperty]

        public string Email { get; internal set; }


        public List<string> UnknownWordsList
        {
            get
            {
                return UnknownWords.Split(',').ToList();
            }
        }

        public List<string> KnownWordsList
        {
            get
            {
                return KnownWords.Split(',').ToList();

            }
        }


    }
}
