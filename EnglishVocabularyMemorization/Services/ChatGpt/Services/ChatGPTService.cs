using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.ChatGpt.Helpers;
using EnglishVocabularyMemorization.Services.ChatGpt.Interfaces;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Completion;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Error;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Transcriptions;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly ChatGPTHelper _chatGPTHelper;
        private readonly string _baseUrl;
        private static HttpClient _httpClient;
        private readonly ApplicationContext _applicationContext;
        public ChatGPTService(ChatGPTHelper chatGPTHelper, ApplicationContext applicationContext)
        {
            _chatGPTHelper = chatGPTHelper;
            _baseUrl = _chatGPTHelper.GetBaseUrl();
            _httpClient = new HttpClient();
            _chatGPTHelper.GetHeader().ToList().ForEach(header => _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value));
            _applicationContext = applicationContext;
        }


        //private static HttpClient _httpClient;
        private static HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());

        public async Task<BaseResult<ChatCompletionResponse>> CompletionsAsync(ChatCompletionRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(
                $"{_baseUrl}/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return BaseResult<ChatCompletionResponse>.CreateValidResult(data: JsonConvert.DeserializeObject<ChatCompletionResponse>(result));
            }
            else
            {
                var contentError = string.Empty;
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(responseStream))
                {
                    var jsonContentError = await streamReader.ReadToEndAsync();
                    var error = JsonConvert.DeserializeObject<RootError>(jsonContentError);
                    contentError = error?.error?.message;
                }

                return BaseResult<ChatCompletionResponse>.CreateInvalidResult($"Error: {contentError}");
            }
        }


        public async Task<BaseResult<ChatCompletionResponse>> GenerateSentences(string word)
        {
            var request = new ChatCompletionRequest();
            request.Model = "gpt-3.5-turbo";
            request.Messages = new List<Models.Shared.Message>();
            //var msg = $"context: The following is a fictional conversation between the USER and Dudi, an AI friend inside the Dudi app who is creative,helpful, and witty.:\n\nDudi: Hi there!,Let's get started. What's on your mind?\nUSER: Write 20 sentences to show the use of the following word: {word}. These sentences must be in portuguese. it is important that the word {word} is in portuguese in each sentence, the translation of the word, that's what I mean, so I can try to translate back to english, please don't make up unexistent meaning\n\n-----------------\n\nyour task: Return a JSON node in the format of" + "{ \"Sentences\": \"[]\", \"UsedMeanings\":\"[...]\"} where\n-Sentences: Short messages containing the word provided in portugues, the work must be surrounded with * *, that Dudi would say in response to the USER to keep the conversation going. Use language that is understandable by an ESL learner at UpperIntermediate level.\n-UsedMeanings: All the meaning used for the word " + word + ".";
            var msg = $"context: The following is a fictional conversation between the USER and Dudi, an AI friend inside the Dudi app who is creative,helpful, and witty.:\n\nDudi: Hi there!,Let's get started. What's on your mind?\nUSER: Write 10 sentences to show the use of the following word: '{word}'. Try to generate them in differente ways with different meanings.\n\n-----------------\n\nyour task: Return a JSON node in the format of" + "{ \"Sentences\": \"[]\", \"UsedMeanings\":\"[...]\",  \"TranslatedSentences\":\"[...]\"} where\n-Sentences: Short messages containing the word provided in portugues that Dudi would say in response to the USER to keep the conversation going. Use language that is understandable by an ESL learner at UpperIntermediate level.\n-UsedMeanings: All the meaning used for the word '" + word +"'.\n-TranslatedSentences: The generated sentences translated to portuguese";
            request.Messages.Add(new Models.Shared.Message { Role = "system", Content= msg });
            var content = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore}), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(
                $"{_baseUrl}/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return BaseResult<ChatCompletionResponse>.CreateValidResult(data: JsonConvert.DeserializeObject<ChatCompletionResponse>(result));
            }
            else
            {
                var contentError = string.Empty;
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(responseStream))
                {
                    var jsonContentError = await streamReader.ReadToEndAsync();
                    var error = JsonConvert.DeserializeObject<RootError>(jsonContentError);
                    contentError = error?.error?.message;
                }

                return BaseResult<ChatCompletionResponse>.CreateInvalidResult($"Error: {contentError}");
            }
        }

        public async Task<BaseResult<ChatCompletionResponse>> CheckAnswers(string sentence, string answer)
        {
            var request = new ChatCompletionRequest();
            request.Model = "gpt-3.5-turbo";
            request.Messages = new List<Models.Shared.Message>();
            var msg = $"context: The is following is an translation answer provided by a student, they translated from portuguese to english, check their answer.:\n\nOriginalSentence: {sentence}\nAnswer: {answer}. \n\n-----------------\n\nyour task: Return a JSON node in the format of" + "{ \"IsCorrect\": \"true/false\", \"Mistakes\":\"[\"\", \"\", .. ]\"} where\n-IsCorrect: it is a boolean(true or false) for the translation correctness.\n-Mistakes: An array of strings pointing out the mistakes commited by the student and why is wrong and informing the correct way, each row of the array must surround by double quotes(important)";
            request.Messages.Add(new Models.Shared.Message { Role = "system", Content = msg });
            var content = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(
                $"{_baseUrl}/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return BaseResult<ChatCompletionResponse>.CreateValidResult(data: JsonConvert.DeserializeObject<ChatCompletionResponse>(result));
            }
            else
            {
                var contentError = string.Empty;
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(responseStream))
                {
                    var jsonContentError = await streamReader.ReadToEndAsync();
                    var error = JsonConvert.DeserializeObject<RootError>(jsonContentError);
                    contentError = error?.error?.message;
                }

                return BaseResult<ChatCompletionResponse>.CreateInvalidResult($"Error: {contentError}");
            }
        }

        //public async Task<BaseResult<TranscriptionsResponse>> TranscriptionsAsync(TranscriptionsRequest request)
        //{
        //    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _chatGPTHelper.GetApiKey());

        //    var content = new MultipartFormDataContent();
        //    content.Add(new StreamContent(request.File), "file", request.FormFile.FileName);
        //    content.Add(new StringContent("whisper-1"), "model");

        //    var response = await HttpClient.PostAsync($"{_baseUrl}/v1/audio/transcriptions", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsJsonAsync<TranscriptionsResponse>();
        //        return BaseResult<TranscriptionsResponse>.CreateValidResult(result);
        //    }
        //    else
        //    {
        //        var contentError = string.Empty;
        //        using (var responseStream = await response.Content.ReadAsStreamAsync())
        //        using (var streamReader = new StreamReader(responseStream))
        //        {
        //            var jsonContentError = await streamReader.ReadToEndAsync();
        //            var error = JsonConvert.DeserializeObject<RootError>(jsonContentError);
        //            contentError = error?.error?.message;
        //        }

        //        return BaseResult<TranscriptionsResponse>.CreateInvalidResult(response.StatusCode, $"Error: {contentError}");
        //    }
        //}
    }
}
