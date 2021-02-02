using AuthorizationClientApp.Shared;
using System.Threading.Tasks;

namespace AuthorizationClientApp.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<RegisterResult> Register(RegisterModel registerModel);
    }
}