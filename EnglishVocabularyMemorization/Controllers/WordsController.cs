using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services;
using EnglishVocabularyMemorization.Services.ChatGpt.Interfaces;
using EnglishVocabularyMemorization.Services.Wordup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly ISpacedRepetitionService _spacedRepetitionService;
        private readonly IWordupService _wordupService;
        private readonly IChatGPTService _chatGptService;
        public WordsController(IWordupService wordupService, ISpacedRepetitionService spacedRepetitionService, IChatGPTService chatGptService)
        {
            _wordupService = wordupService;
            _spacedRepetitionService = spacedRepetitionService;
            _chatGptService = chatGptService;
        }

        [HttpGet("toreview")]
        public async Task<IActionResult> GetWordsToReview([FromQuery] string email)
        {
            var result = await _spacedRepetitionService.GetWordsToReview(email);
            return Ok(result);
        }

        [HttpGet("reviewing")]
        public async Task<IActionResult> GetWordsInReviewProcess([FromQuery] string email)
        {
            var result = await _spacedRepetitionService.GetWordsInRepetitionProcess(email);
            return Ok(result);
        }

        [HttpGet("next/{wordId}")]
        public async Task<IActionResult> NextWord([FromQuery] string email, [FromRoute] string wordId)
        {
            var result = await _spacedRepetitionService.SetWordReviewed(wordId, email);
            return Ok(result);
        }

        [HttpGet("open/{wordId}")]
        public async Task<IActionResult> OpenWord([FromQuery] string email, [FromRoute]string wordId)
        {
            var result = await _spacedRepetitionService.OpenWord(wordId, email);

            return Ok(result);
        }

        [HttpGet("sentences/{word}")]
        public async Task<IActionResult> GenerateSentences([FromQuery] string email, [FromRoute] string word, [FromQuery] bool ai)
        {
            var savedSentences =await _spacedRepetitionService.GetSavedSentences(email, word);
            var sentences = new List<dynamic>();

            if (ai)
            {
                var result = await _chatGptService.GenerateSentences(word);

                if (result.IsSuccess)
                {
                    var json = result.Data.Choices.First().Message.Content;
                    var dynamic = JsonConvert.DeserializeObject<dynamic>(json);

                    foreach (var item in dynamic.TranslatedSentences)
                    {
                        sentences.Add(new { id = Guid.NewGuid(), sentence = item.Value });
                    }

                }

                return Ok(BaseResult<dynamic>.CreateValidResult(sentences));

            }


            foreach (var savedSentence in savedSentences.Data)
            {
                sentences.Add(new { id = Guid.NewGuid(), sentence = savedSentence.Text, answers = savedSentence.LastAnswers });
            }

            return Ok(BaseResult<dynamic>.CreateValidResult(sentences));


            return BadRequest(BaseResult<dynamic>.CreateInvalidResult("Error trying to generate answers"));
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckAnswer([FromBody]CheckRequest request)
        {
            var result = await _chatGptService.CheckAnswers(request.Sentence, request.Answer);
            if (result.IsSuccess)
            {
                var json = result.Data.Choices.First().Message.Content;
                var dynamic = JsonConvert.DeserializeObject<CheckResponse>(json);
                var sentences = new List<dynamic>();

                return Ok(BaseResult<CheckResponse>.CreateValidResult(dynamic));
            }
            return BadRequest(BaseResult<CheckResponse>.CreateInvalidResult("Error trying to generate answers"));
        }

        [HttpPost("saveSentence")]
        public async Task<IActionResult> SaveSentence([FromQuery] string email,[FromQuery] string wordId, [FromBody] SaveQuestion question)
        {
            var result = await _spacedRepetitionService.SaveSentence(question.Sentence, question.Answer, wordId);
            if (result.IsSuccess)
            {
                return Ok(BaseResult<bool>.CreateValidResult(true));
            }
            return BadRequest(BaseResult<CheckResponse>.CreateInvalidResult("Error trying to generate answers"));
        }
    }

    public class CheckRequest
    {
        public string Sentence { get; set; }
        public string Answer { get; set; }
    }

    public class SaveQuestion
    {
        public string Sentence { get; set; }
        public string Answer { get; set; }
    }

    public class CheckResponse
    {
        public bool IsCorrect { get; set; }
        public string[] Mistakes { get; set; }
    }
}
