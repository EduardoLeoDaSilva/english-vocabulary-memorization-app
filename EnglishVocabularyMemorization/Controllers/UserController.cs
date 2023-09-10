using EnglishVocabularyMemorization.Services;
using EnglishVocabularyMemorization.Services.Wordup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocabularyMemorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISpacedRepetitionService _spacedRepetitionService;
        private readonly IWordupService _wordupService;
        private readonly IUserService _userService;

        public UserController(ISpacedRepetitionService spacedRepetitionService, IWordupService wordupService, IUserService userService)
        {
            _spacedRepetitionService = spacedRepetitionService;
            _wordupService = wordupService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo([FromQuery]string email)
        {
            var result = await _userService.GetUserInfo(email);
            return Ok(result);
        }

        [HttpGet("/{pin}")]
        public async Task<IActionResult> SetUserPin([FromRoute]string pin)
        {
            var result = await _userService.SetUserPin(pin);
            return Ok(result);
        }
    }
}
