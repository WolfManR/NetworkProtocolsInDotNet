namespace ClinicService.Data.Storage;

public class PetRepository : BaseRepository<Pet, int>, IPetRepository
{
    public PetRepository(ClinicContext context) : base(context)
    {
    }

    public override void Update(Pet item)
    {
        if (GetById(item.Id) is not { } entity) throw new KeyNotFoundException();

        entity.Name = item.Name;
        entity.Birthday = item.Birthday;

        _context.SaveChanges();
    }
}