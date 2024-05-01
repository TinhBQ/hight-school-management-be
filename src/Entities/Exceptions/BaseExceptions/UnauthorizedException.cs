namespace DomainModel.Exceptions.BaseExceptions
{
    public class UnauthorizedException(string message) : Exception($"Không có quyền truy cập: '{message}'")
    {
    }
}
