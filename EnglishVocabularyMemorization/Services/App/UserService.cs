using EnglishVocabularyMemorization.Entities;
using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.Wordup;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocabularyMemorization.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly IWordupService _wordupService;
        public UserService(ApplicationContext context, IWordupService wordupService)
        {
            _context = context;
            _wordupService = wordupService;
        }
        public async Task<BaseResult<List<Word>>> SetUserPin(string pin)
        {
            var resultWordUp = await _wordupService.GetUserWords(pin);
            var wordsFromWordUp = await _wordupService.GetWords(null);
            var wordsToSave = wordsFromWordUp.Data.Where(x => resultWordUp.Data.UnknownWordsList.Contains(x.WordUpId)).ToList();
            var user = _context.Users.FirstOrDefault(x => x.Pin == pin);
            if (user == null)
            {
                user = new Entities.User { Id = Guid.NewGuid(), Email = resultWordUp.Data.Email, Pin = pin };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            if (wordsToSave != null && wordsToSave.Count > 0)
            {
                var wordsFromDb = await _context.Words.Include(x => x.User).Where(x => x.User.Id == user.Id).ToListAsync();
                var wordsToSaveFiltered = wordsToSave.Where(x => !wordsFromDb.Select(d => d.WordUpId).Contains(x.WordUpId)).ToList();
                foreach (var word in wordsToSaveFiltered)
                {
                    word.LastTimeReviewed = new DateTime();
                    word.TimesReviewed = 0;
                    word.User = user;
                    _context.Words.Add(word);
                }
            }
            await _context.SaveChangesAsync();

            var wordsToReturn = await _context.Words.Include(x => x.User).Where(x => x.User.Pin == pin).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsToReturn);
        }

        public async Task<BaseResult<List<Word>>> GetUserInfo(string email)
        {
            var wordsToReturn = await _context.Words.Include(x => x.User).Where(x => x.User.Email == email).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsToReturn);

        }

    }
}
