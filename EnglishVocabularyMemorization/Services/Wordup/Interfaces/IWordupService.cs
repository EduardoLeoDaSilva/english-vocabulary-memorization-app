using EnglishVocabularyMemorization.Entities;
using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.Wordup.Models;

namespace EnglishVocabularyMemorization.Services.Wordup
{
    public interface IWordupService
    {
        Task<BaseResult<UserWords>> GetUserWords(string pin);
        Task<BaseResult<List<Word>>> GetWords(List<string>? ids);
    }
}