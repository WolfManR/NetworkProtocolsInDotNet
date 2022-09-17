namespace ClinicService.Models;

public class AuthenticationRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}