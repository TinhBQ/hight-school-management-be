namespace Services.Abstraction.IApplicationServices
{
    public interface IAuthenticationService
    {
        public bool Login();

        public bool Logout();

        public bool Register();

        public bool RenewToken();
    }
}
