namespace ClinicService.Data;

public class AccountSession
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;

    public DateTime TimeCreated { get; set; }
    public DateTime TimeLastRequest { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? TimeClosed { get; set; }

    public int AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;
}