namespace Entities.Responses
{
    public class ResponseBase<T>
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; } = "";
        public T? Data { get; set; }
    }
}
