using EnglishVocabularyMemorization.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EnglishVocabularyMemorization.Services.ChatGpt.Helpers
{
    public class ChatGPTHelper
    {
        private readonly IConfiguration _configuration;
        private ApplicationContext _context;
        public ChatGPTHelper(IConfiguration configuration, ApplicationContext context)
        {
            _configuration = configuration;
            _context = context;
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
            var apiKey = _context.Set<Config>().Where(x => x.Key == "ChatGPTKey").FirstOrDefault();
            var accessToken = apiKey.Value;
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
