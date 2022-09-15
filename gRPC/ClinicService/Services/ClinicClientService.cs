using System.Text.Json;

using ClinicService.Data;
using ClinicService.Data.Storage;

using ClinicServiceProtos;

using Grpc.Core;

using static ClinicServiceProtos.ClinicClientService;

namespace ClinicService.Services;

public class ClinicClientService : ClinicClientServiceBase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<ClinicClientService> _logger;

    public ClinicClientService(IClientRepository clientRepository, ILogger<ClinicClientService> logger)
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
    {
        Client client = new()
        {
            Document = request.Document,
            FirstName = request.FirstName,
            Surname = request.Surname,
            Patronymic = request.Patronymic
        };

        var id = _clientRepository.Add(client);

        _logger.LogInformation("Client created: {ClientData}", JsonSerializer.Serialize(client));

        CreateClientResponse response = new() { ClientId = id };

        return Task.FromResult(response);
    }

    public override Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
    {
        Client edited = new()
        {
            Id = request.ClientId,
            Document = request.Document,
            FirstName = request.FirstName,
            Surname = request.Surname,
            Patronymic = request.Patronymic
        };
        // How to validate exceptions and return it response
        _clientRepository.Update(edited);

        return Task.FromResult(new UpdateClientResponse());
    }

    public override Task<DeleteClientResponse> DeleteClient(DeleteClientRequest request, ServerCallContext context)
    {
        _clientRepository.Delete(request.ClientId);

        return Task.FromResult(new DeleteClientResponse());
    }

    public override Task<ClientResponse> GetClientById(GetClientByIdRequest request, ServerCallContext context)
    {
        var entity = _clientRepository.GetById(request.ClientId);
        ClientResponse response = entity is null ? new() : Map(entity);
        return Task.FromResult(response);
    }

    public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
    {
        var entities = _clientRepository.GetAll();
        GetClientsResponse response = new();
        if (entities.Count > 0) response.Clients.AddRange(entities.Select(e => Map(e)));
        return Task.FromResult(response);
    }

    private static ClientResponse Map(Client data) => new()
    {
        ClientId = data.Id,
        Document = data.Document,
        FirstName = data.FirstName,
        Surname = data.Surname,
        Patronymic = data.Patronymic
    };
}