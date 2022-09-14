namespace ClinicService.Data.Storage;

public class PetRepository : BaseRepository<Pet, int>, IPetRepository
{
    public PetRepository(ClinicContext context) : base(context)
    {
    }
}