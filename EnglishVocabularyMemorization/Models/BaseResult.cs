namespace EnglishVocabularyMemorization.Models
{
    public class BaseResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }

        public static BaseResult<T> CreateInvalidResult(string error)
        {
            var result = new BaseResult<T>();
            result.Errors.Add(error);
            result.IsSuccess = false;
            return result;
        }

        public static BaseResult<T> CreateValidResult(T data)
        {
            var result = new BaseResult<T>();
            result.Data = data;
            result.IsSuccess = true;
            return result;
        }

        public BaseResult<T> AddError(string error)
        {       
            this.Errors.Add(error);
            this.IsSuccess = false;
            return this;
        }

        public  BaseResult<T> ValidResult(T data)
        {
            this.Data = data;
            this.IsSuccess = false;
            return this;
        }
    }
}
