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

        public async Task<BaseResult<bool>> AddWord(string word, string email, string definition, User user)
        {
            if (_applicationService.Words.Any(x => x.Name.ToLower() == word.ToLower()))
            {
                return BaseResult<bool>.CreateValidResult(true);
            }

            var wordID = "add" +  DateTime.Now.Ticks.ToString();

            var wordsDb = await _applicationService.Words.AddAsync(new Word { Id = Guid.NewGuid(), WordUpId = wordID, Definition = definition, Name = word, User =user  });
            await _applicationService.SaveChangesAsync();
            return BaseResult<bool>.CreateValidResult(true);

        }

        public async Task<BaseResult<List<Word>>> GetWordsInRepetitionProcess(string email)
        {
            var wordsDb = await _applicationService.Words.Include(x => x.User).Where(x => x.TimesReviewed > 0 && x.User.Email == email).OrderBy(x => x.LastTimeReviewed).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsDb);
        }

        public async Task<BaseResult<List<Sentence>>> GetSavedSentences(string email, string word)
        {
            var sentences = await _applicationService.Sentences.Include(x => x.Word).ThenInclude(x => x.User).Where(x => x.Word.Name == word && x.Word.User.Email == email).ToListAsync();
            return BaseResult<List<Sentence>>.CreateValidResult(sentences);
        }

        public async Task<BaseResult<List<Word>>> GetWordsInRepetitionToReview(string email)
        {
            var wordsDb = await _applicationService.Words.Include(x => x.User).Where(x => x.TimesReviewed > 0 && x.User.Email == email && (x.LastTimeReviewed - DateTime.Now).TotalDays <= 0).OrderByDescending(x => x.LastTimeReviewed).ToListAsync();
            return BaseResult<List<Word>>.CreateValidResult(wordsDb);
        }

        public async Task<BaseResult<bool>> SaveSentence(string sentence, string answer, string originalSentence, string wordId)
        {
            var word = await _applicationService.Words.Where(x => x.WordUpId == wordId).FirstOrDefaultAsync();
            var dbSentence = await _applicationService.Sentences.Where(x => x.Text == sentence).FirstOrDefaultAsync();

            if (dbSentence == null)
            {
                dbSentence = new Sentence { Id = Guid.NewGuid(), Text = sentence, OriginalSentence = originalSentence, LastAnswers = new List<string> { answer }, WordId = word.Id };
                await _applicationService.Sentences.AddAsync(dbSentence);
            }
            else
            {
                dbSentence.LastAnswers.Add(answer);
                _applicationService.Sentences.Update(dbSentence);

            }

            await _applicationService.SaveChangesAsync();

            return BaseResult<bool>.CreateValidResult(true);

        }

        public async Task<BaseResult<Exam>> GenerateExam(string email)
        {
            var exam = await _applicationService
                .Exams
                .Include(x => x.Sentences)
                .ThenInclude(x => x.Word)
                .Where(x => x.Email == email && x.IsFinished == false).FirstOrDefaultAsync();

            if (exam != null)
                return BaseResult<Exam>.CreateValidResult(exam);

            var sentences = await _applicationService.Sentences
                .Include(x => x.Word)
                .ThenInclude(x => x.User)
                .Where(x => x.Word.User.Email == email)
                .Where(x => x.Word.LastExam == DateTime.MinValue || EF.Functions.DateDiffDay(x.Word.LastExam, DateTime.Now)  >= 2).ToListAsync();


            if(!sentences.Any())
                return BaseResult<Exam>.CreateInvalidResult("Sem exames para criar");


            var sentenceGroupped = sentences.GroupBy(x => x.WordId).Take(5);

            var sentecesToReturn = sentenceGroupped.SelectMany(x => x.Select(s => s)).ToList();

            if (!sentences.Any())
                return BaseResult<Exam>.CreateInvalidResult("Sem exames para criar");


            var newExam = new Exam { Id = Guid.NewGuid(), Email = email, IsFinished = false, Sentences = sentences };

            await _applicationService.Exams.AddAsync(newExam);
            await _applicationService.SaveChangesAsync();

            return BaseResult<Exam>.CreateValidResult(newExam);

        }

        public async Task<BaseResult<bool>> FinishExam(string id)
        {
            var exam = await _applicationService.Exams.Include(x => x.Sentences).ThenInclude(x => x.Word).Where(x => x.Id.ToString() == id).FirstOrDefaultAsync();
            exam.LastExam = DateTime.Now;
            exam.Sentences.ForEach(x => x.Word.LastExam = DateTime.Now);
            exam.IsFinished = true;
            await _applicationService.SaveChangesAsync();
            return BaseResult<bool>.CreateValidResult(true);
        }

        public async Task<BaseResult<Word>> OpenWord(string wordId, string email)
        {
            var wordFromWorduUpResult = await _wordupService.GetWords(new List<string> { wordId });
            var wordFromWordup = wordFromWorduUpResult.Data.FirstOrDefault();
            var wordDb = await _applicationService.Words.FirstOrDefaultAsync(x => x.WordUpId == wordId);
            var user = await _applicationService.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (wordDb == null)
            {
                wordDb = new Word
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    TimesReviewed = 0,
                    LastTimeReviewed = new DateTime()
                };

                if(wordFromWordup != null)
                {
                    wordDb.Definition = wordFromWordup.Definition;
                    wordDb.Name = wordFromWordup.Name;
                    wordDb.WordUpId = wordFromWordup.WordUpId;
                }


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
