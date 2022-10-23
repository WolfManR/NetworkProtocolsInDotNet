namespace ClinicService.Models;

public class SessionContext
{
    public int SessionId { get; set; }
    public string SessionToken { get; set; } = null!;
    public AccountDto Account { get; set; } = null!;
}