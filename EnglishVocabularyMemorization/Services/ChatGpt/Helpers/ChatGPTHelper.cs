using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Helpers
{
    public class ChatGPTHelper
    {
        private readonly IConfiguration _configuration;
        public ChatGPTHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Pega url base da API
        /// </summary>
        /// <returns></returns>
        public string GetBaseUrl()
        {
            return _configuration.GetValue<string>("ChatGPT:BaseUrl");
        }

        /// <summary>
        /// Monta o cabeçalho com token de autenticação
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetHeader()
        {
            var header = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {this.GetApiKey()}" }
            };
            return header;
        }

        /// <summary>
        /// Obtém o token de autenticação
        /// </summary>
        /// <returns></returns>
        public  string GetApiKey()
        {
            var accessToken = _configuration.GetValue<string>("ChatGPT:ApiKey");
            return accessToken;
        }

        /// <summary>
        /// Pega a configuração de serialização da requisição
        /// </summary>
        ///// <returns></returns>
        //public JsonMediaTypeFormatter GetJsonMediaTypeFormatter()
        //{
        //    var formatter = new JsonMediaTypeFormatter()
        //    {
        //        SerializerSettings = new JsonSerializerSettings
        //        {
        //            Formatting = Formatting.None,
        //            NullValueHandling = NullValueHandling.Ignore,
        //            ObjectCreationHandling = ObjectCreationHandling.Replace
        //        }
        //    };
        //    formatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        //    formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        //    return formatter;
        //}
    }
}
