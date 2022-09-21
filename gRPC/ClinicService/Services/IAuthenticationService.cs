using ClinicService.Models;

namespace ClinicService.Services;

public interface IAuthenticationService
{
    SessionContext? GetSessionInfo(string sessionToken);
    AuthenticationResponse Login(AuthenticationRequest authenticationRequest);
}