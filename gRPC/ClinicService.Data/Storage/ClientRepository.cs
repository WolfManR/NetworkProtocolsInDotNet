namespace ClinicService.Data.Storage;

public class ClientRepository : BaseRepository<Client, int>,IClientRepository
{
    public ClientRepository(ClinicContext context) : base(context)
    {
    }
}