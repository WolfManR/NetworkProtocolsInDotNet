namespace ClinicService.Data.Storage;

public class ConsultationRepository : BaseRepository<Consultation, int>, IConsultationRepository
{
    public ConsultationRepository(ClinicContext context) : base(context)
    {
    }
}