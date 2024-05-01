namespace DomainModel.Exceptions.BaseExceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message) : base($"Yêu cầu bị gián đoạn: '{message}'")
        {
        }
    }
}
