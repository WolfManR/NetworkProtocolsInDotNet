namespace ClinicService.Data;

public class Account
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool Locked { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SecondName { get; set; }
    public virtual ICollection<AccountSession> Sessions { get; set; } = new HashSet<AccountSession>();
}