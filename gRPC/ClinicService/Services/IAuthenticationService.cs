using ClinicService.Models;

namespace ClinicService.Services;

public interface IAuthenticationService
{
    SessionContext? GetSessionInfo(string sessionToken);
    AuthenticationResult Login(string login, string password);
}