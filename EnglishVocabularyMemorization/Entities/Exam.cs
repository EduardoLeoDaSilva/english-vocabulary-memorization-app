namespace EnglishVocabularyMemorization.Entities
{
    public class Exam
    {
        public Guid Id { get; set; }
        public List<Sentence> Sentences { get; set; }
        public string Email { get; set; }
        public bool IsFinished { get; set; } = false;

        public DateTime? LastExam { get; set; }

    }
}
