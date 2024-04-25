namespace Services.Abstraction.IRepositoryServices
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
