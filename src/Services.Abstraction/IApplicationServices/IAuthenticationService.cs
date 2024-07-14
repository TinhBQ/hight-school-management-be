namespace Services.Abstraction.IApplicationServices
{
    public interface IAuthenticationService
    {
        public Task<Dictionary<string, string>> Login(string username, string password);

        public Task Logout();

        public Task RenewToken();

        public Task ChangePassword(Guid userId, string oldPassword, string newPassword);

        public Task GenerateAccount(List<Guid> ids);
    }
}
