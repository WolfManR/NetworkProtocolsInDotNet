namespace ClinicService.Data.Storage;

public class ClientRepository : BaseRepository<Client, int>,IClientRepository
{
    public ClientRepository(ClinicContext context) : base(context)
    {
    }

    public override void Update(Client item)
    {
        if (GetById(item.Id) is not { } entity) throw new KeyNotFoundException();

        entity.Document = item.Document;
        entity.FirstName = item.FirstName;
        entity.Surname = item.Surname;
        entity.Patronymic = item.Patronymic;

        _context.SaveChanges();
    }
}