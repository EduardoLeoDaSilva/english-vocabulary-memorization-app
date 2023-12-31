﻿using EnglishVocabularyMemorization.Services.ChatGpt.Models.Shared;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Models.Completion
{
    public class ChatCompletionResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }
    }
}
