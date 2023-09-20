using EnglishVocabularyMemorization.Entities;
using EnglishVocabularyMemorization.Models;

namespace EnglishVocabularyMemorization.Services
{
    public interface ISpacedRepetitionService
    {
        Task<BaseResult<Word>> OpenWord(string wordId, string pin);
        Task<BaseResult<Word>> SetWordReviewed(string wordId, string email);
        Task<BaseResult<List<Word>>> GetWordsToReview(string email);
        Task<BaseResult<List<Word>>> GetWordsInRepetitionProcess(string email);
        Task<BaseResult<bool>> SaveSentence(string sentence, string answer, string wordId);
        Task<BaseResult<List<Word>>> GetWordsInRepetitionToReview(string email);
        Task<BaseResult<List<Sentence>>> GetSavedSentences(string email, string word);
    }
}