namespace EnglishVocabularyMemorization.Services.ChatGpt.Models.Shared
{
    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string Finish_reason { get; set; }
    }
}
