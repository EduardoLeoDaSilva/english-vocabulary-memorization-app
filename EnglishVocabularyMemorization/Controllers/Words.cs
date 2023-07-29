using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocabularyMemorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Words : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWordsToReview()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetWordsInReviewProcess()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> NextWord()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> OpenWord()
        {
            return Ok();
        }
    }
}
