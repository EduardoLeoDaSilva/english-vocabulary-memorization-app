using EnglishVocabularyMemorization.Entities;
using EnglishVocabularyMemorization.Models;

namespace EnglishVocabularyMemorization.Services
{
    public interface IUserService
    {
        Task<BaseResult<List<Word>>> GetUserInfo(string email);
        Task<BaseResult<List<Word>>> SetUserPin(string pin);
        Task<BaseResult<User>> GetUser(string email);
    }
}