using System.Text.Json.Serialization;

namespace EnglishVocabularyMemorization.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Pin { get; set; }
        [JsonIgnore]
        public List<Word> Words { get; set; }
    }
}
