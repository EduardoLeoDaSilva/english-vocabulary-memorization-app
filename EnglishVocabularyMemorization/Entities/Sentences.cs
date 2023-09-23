using System.Text.Json.Serialization;

namespace EnglishVocabularyMemorization.Entities
{
    public class Sentence
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<string> LastAnswers { get; set; }

        public Word Word { get; set; }

        public Guid WordId { get; set; }

        public string OriginalSentence { get; set; }

        [JsonIgnore]
        public List<Exam> Exams { get; set; }

    }
}
