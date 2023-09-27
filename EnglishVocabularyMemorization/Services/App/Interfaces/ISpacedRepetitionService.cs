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
        Task<BaseResult<bool>> SaveSentence(string sentence, string answer, string originalSentence , string wordId);
        Task<BaseResult<List<Word>>> GetWordsInRepetitionToReview(string email);
        Task<BaseResult<List<Sentence>>> GetSavedSentences(string email, string word);
        Task<BaseResult<Exam>> GenerateExam(string email);
        Task<BaseResult<bool>> FinishExam(string id);

        Task<BaseResult<bool>> AddWord(string word, string email, string definition, User user);
    }
}