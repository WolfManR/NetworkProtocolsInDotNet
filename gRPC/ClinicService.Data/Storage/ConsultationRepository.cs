namespace ClinicService.Data.Storage;

public class ConsultationRepository : BaseRepository<Consultation, int>, IConsultationRepository
{
    public ConsultationRepository(ClinicContext context) : base(context)
    {
    }

    public override void Update(Consultation item)
    {
        if (GetById(item.Id) is not { } entity) throw new KeyNotFoundException();

        entity.Description = item.Description;
        entity.ConsultationDate = item.ConsultationDate;

        _context.SaveChanges();
    }
}