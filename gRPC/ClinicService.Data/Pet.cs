namespace ClinicService.Data;

public class Pet : IEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public DateTime Birthday { get; set; }

    public int ClientId { get; set; }
    public virtual Client Client { get; set; } = null!;
    public virtual ICollection<Consultation> Consultations { get; set; } = new HashSet<Consultation>();
}