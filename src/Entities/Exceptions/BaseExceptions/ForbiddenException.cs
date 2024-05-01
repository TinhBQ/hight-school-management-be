namespace DomainModel.Exceptions.BaseExceptions
{
    public class ForbiddenException(string message) : Exception($"Truy cập bị chặn: '{message}'")
    {
    }
}
