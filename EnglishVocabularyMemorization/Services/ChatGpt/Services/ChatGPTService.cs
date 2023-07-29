using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.ChatGpt.Helpers;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Completion;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Error;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Transcriptions;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Services
{
    public class ChatGPTService
    {
        private readonly ChatGPTHelper _chatGPTHelper;
        private readonly string _baseUrl;
        private static HttpClient _httpClient;
        public ChatGPTService(ChatGPTHelper chatGPTHelper)
        {
            _chatGPTHelper = chatGPTHelper;
            _baseUrl = _chatGPTHelper.GetBaseUrl();
            _httpClient = new HttpClient();
            _chatGPTHelper.GetHeader().ToList().ForEach(header => _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value));
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
