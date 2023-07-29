namespace EnglishVocabularyMemorization.Entities
{
    public class Word
    {
        public Guid Id { get; set; }
        public string WordUpId { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }

        public int TimeReviewed { get; set; }
        public DateTime LastTimeReviewed { get; set; }
    }
}
