namespace ClinicService.Models;

public class AuthenticationResponse
{
    public AuthenticationStatus Status { get; set; }

    public SessionContext SessionContext { get; set; } = null!;

    public static AuthenticationResponse UserNotFound() => 
        new() { Status = AuthenticationStatus.UserNotFound };
    public static AuthenticationResponse InvalidPassword() => 
        new() { Status = AuthenticationStatus.InvalidPassword };
    public static AuthenticationResponse Success(SessionContext sessionContext) => 
        new() { Status = AuthenticationStatus.Success, SessionContext = sessionContext};
}