namespace Entities.Responses
{
    public class ResponseBase<T> where T : class
    {
        public ResponseBase()
        {
        }
        public ResponseBase(T data, string message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public ResponseBase(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public List<string> Errors { get; set; } = null!;
        public T Data { get; set; } = null!;
    }
}
