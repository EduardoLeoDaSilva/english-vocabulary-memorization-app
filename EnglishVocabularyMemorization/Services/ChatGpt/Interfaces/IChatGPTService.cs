using EnglishVocabularyMemorization.Models;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Completion;
using EnglishVocabularyMemorization.Services.ChatGpt.Models.Transcriptions;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Interfaces
{
    public interface IChatGPTService
    {
        /// <summary>
        /// Cria uma resposta de modelo para a conversa de bate-papo fornecida.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResult<ChatCompletionResponse>> CompletionsAsync(ChatCompletionRequest request);

        Task<BaseResult<ChatCompletionResponse>> GenerateSentences(string word);

        Task<BaseResult<ChatCompletionResponse>> CheckAnswers(string sentence, string answer);

        Task<BaseResult<ChatCompletionResponse>> GenerateDefinition(string word);
        /// <summary>
        /// Transcreve o áudio para o idioma de entrada.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //Task<BaseResult<TranscriptionsResponse>> TranscriptionsAsync(TranscriptionsRequest request);
    }
}
