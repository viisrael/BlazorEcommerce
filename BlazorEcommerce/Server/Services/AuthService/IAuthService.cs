namespace BlazorEcommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> UserExists(string email);
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
    }
}
