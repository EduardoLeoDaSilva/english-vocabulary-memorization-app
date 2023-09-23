using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace EnglishVocabularyMemorization.Entities
{
    public class Word
    {
        public Guid Id { get; set; }
        public string WordUpId { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }

        public int TimesReviewed { get; set; }
        public DateTime LastTimeReviewed { get; set; }

        public User User { get; internal set; }

        public DateTime LastExam { get; set; }

        [JsonIgnore]
        public List<Sentence> SavedSentences { get; set; }

        [NotMapped]
        public string NextReview
        {
            get
            {
                var date = LastTimeReviewed - DateTime.Now;
                return date.Days.ToString();
            }
        }

    }
}
