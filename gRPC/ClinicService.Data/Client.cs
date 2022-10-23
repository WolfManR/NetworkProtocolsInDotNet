namespace ClinicService.Data;

public class Client : IEntity<int>
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;

    public string? Document { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
    public virtual ICollection<Consultation> Consultations { get; set; } = new HashSet<Consultation>();
}