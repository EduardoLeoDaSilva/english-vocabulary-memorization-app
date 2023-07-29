using EnglishVocabularyMemorization.Entities;
using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.Wordup.Models;
using Newtonsoft.Json;
using System.Text;

namespace EnglishVocabularyMemorization.Services.Wordup
{
    public class WordupService
    {
        private readonly HttpClient _httpClient;
        public WordupService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://auth.wordupapp.co");
        }

        public async Task<BaseResult<UserWords>> GetUserWords(string pin)
        {
            var response = await _httpClient.GetAsync($"/ext/verify/{pin}");

            if (response.IsSuccessStatusCode)
                return  BaseResult<UserWords>.CreateValidResult(JsonConvert.DeserializeObject<UserWords>(await response.Content.ReadAsStringAsync()));

            return BaseResult<UserWords>.CreateInvalidResult("Erro ao buscar palavras na wordup");
        }

        public async Task<BaseResult<List<Word>>> GetWords(List<string>? ids)
        {
            var result = "";
            var wordsList = new List<Word>();
            using (StreamReader streamReader = new StreamReader("WordsBase.txt", Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }

            if(result != null && result.Length > 0)
            {
                Parallel.ForEach(result.Split("\n"), wordLine =>
                {
                    var wordSplited = wordLine.Split('|');
                    var wordId = wordSplited[0];
                    var wordRank = wordSplited[1];
                    var wordRoot = wordSplited[2];

                    wordsList.Add(new Word { WordUpId = wordId, Definition= wordRoot });

                });
                return BaseResult<List<Word>>.CreateValidResult(wordsList ?? new List<Word>());

            }
            return BaseResult<List<Word>>.CreateInvalidResult("Erro ao buscar palavras na wordup");
        }
    }
}
