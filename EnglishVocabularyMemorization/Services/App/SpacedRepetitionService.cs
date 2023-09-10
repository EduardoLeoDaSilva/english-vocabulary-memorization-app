using EnglishVocabularyMemorization.Entities;
using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.ChatGpt.Interfaces;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Completion;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Shared;
using EnglishVocabularyMemorization.Services.Wordup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.Services
{
    public class SpacedRepetitionService : ISpacedRepetitionService
    {
        private readonly ApplicationContext _applicationService;
        private readonly IWordupService _wordupService;
        private readonly IChatGPTService _chatGptService;
        public SpacedRepetitionService(ApplicationContext applicationService, IWordupService wordupService, IChatGPTService chatGptService)
        {
            _applicationService = applicationService;
            _wordupService = wordupService;
            _chatGptService = chatGptService;
        }

        public async Task<BaseResult<Word>> SetWordReviewed(string wordId, string email)
        {
            var wordDb = await _applicationService.Words.Include(x => x.User).FirstOrDefaultAsync(x => x.WordUpId == wordId && x.User.Email == email);
            wordDb.LastTimeReviewed = DateTime.Now.AddDays(1 + wordDb.TimesReviewed);
            wordDb.TimesReviewed += 1;
            await _applicationService.SaveChangesAsync();
            return BaseResult<Word>.CreateValidResult(null);
        }

        public async Task<BaseResult<List<Word>>> GetWordsToReview(string email)
        {
            var wordsDb = await _applicationService.Words.Include(x => x.User).Where(x => x.TimesReviewed == 0 && x.User.Email == email).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsDb);

        }

        public async Task<BaseResult<List<Word>>> GetWordsInRepetitionProcess(string email)
        {
            var wordsDb = await _applicationService.Words.Include(x => x.User).Where(x => x.TimesReviewed > 0 && x.User.Email == email).OrderByDescending(x => x.LastTimeReviewed).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsDb);
        }

        public async Task<BaseResult<List<Word>>> GetWordsInRepetitionToReview(string email)
        {
            var wordsDb = await _applicationService.Words.Include(x => x.User).Where(x => x.TimesReviewed > 0 && x.User.Email == email && (x.LastTimeReviewed - DateTime.Now).TotalDays <= 0).OrderByDescending(x => x.LastTimeReviewed).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsDb);
        }


        public async Task<BaseResult<Word>> OpenWord(string wordId, string email)
        {
            var wordFromWorduUpResult = await _wordupService.GetWords(new List<string> { wordId });
            var wordFromWordup = wordFromWorduUpResult.Data.First();
            var wordDb = await _applicationService.Words.FirstOrDefaultAsync(x => x.WordUpId == wordId);
            var user = await _applicationService.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (wordDb == null)
            {
                wordDb = new Word
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    Definition = wordFromWordup.Definition,
                    Name = wordFromWordup.Name,
                    WordUpId = wordFromWordup.WordUpId,
                    TimesReviewed = 0,
                    LastTimeReviewed = new DateTime()
                };

                _applicationService.Words.Add(wordDb);

                await _applicationService.SaveChangesAsync();
            }

            var msg = new Message
            {
                Role = "user",
                Content =
    $"Gere os exercicios para as seguintes palavras em inglês {wordDb.Name} \n" +
" 1 - Criei 20 frases para cada palavras tente abordar mais de um signifado pra elas e classique se é comum, raro no dia a dia, e também fornecça a traduças para elas \n" +
" 2 - Mostre os sinonimos pra elas com traduções \n" +
" 3 - Faça perguntas que me force a utiliza-las e fornceças as traduções das perguntas"
            };

            //var content = JsonConvert.SerializeObject(new List<Message> { msg });
            //var result = await _chatGptService.CompletionsAsync(new ChatCompletionRequest
            //{
            //    Model = "gpt-3.5-turbo",
            //    Messages = new List<Message> { msg}
            //});

            return BaseResult<Word>.CreateValidResult(wordDb);
        }
    }
}
