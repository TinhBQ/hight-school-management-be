namespace Entities.Exceptions.BaseExceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base($"Không tìm thấy: '{message}'")
        { }
    }
}
