using System.Text.Json.Serialization;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Models.Transcriptions
{
    public class TranscriptionsResponse
    {
        public Stream File { get; set; }
        public string Model { get; set; }

        [JsonIgnore]
        public IFormFile FormFile { get; set; }
    }
}
