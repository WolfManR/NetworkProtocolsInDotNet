using ClinicServiceProtos;

namespace ClinicService.Models;

public class AuthenticationResult
{
    public AuthenticationStatus Status { get; set; }

    public SessionContext SessionContext { get; set; } = null!;

    public static AuthenticationResult UserNotFound() => 
        new() { Status = AuthenticationStatus.UserNotFound };
    public static AuthenticationResult InvalidPassword() => 
        new() { Status = AuthenticationStatus.InvalidPassword };
    public static AuthenticationResult Success(SessionContext sessionContext) => 
        new() { Status = AuthenticationStatus.Success, SessionContext = sessionContext};
}