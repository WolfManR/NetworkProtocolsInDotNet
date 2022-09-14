namespace ClinicService.Data;

public class Consultation
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;
    public DateTime ConsultationDate { get; set; }

    public int ClientId { get; set; }
    public int PetId { get; set; }
    public virtual Client Client { get; set; } = null!;
    public virtual Pet Pet { get; set; } = null!;
}