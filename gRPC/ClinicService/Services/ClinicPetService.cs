using ClinicService.Data;
using ClinicService.Data.Storage;

using ClinicServiceProtos;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using static ClinicServiceProtos.ClinicPetService;

namespace ClinicService.Services;

public class ClinicPetService : ClinicPetServiceBase
{
    private readonly IPetRepository _repository;
    private readonly ILogger<ClinicPetService> _logger;

    public ClinicPetService(IPetRepository repository, ILogger<ClinicPetService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public override Task<CreatePetResponse> CreatePet(CreatePetRequest request, ServerCallContext context)
    {
        Pet pet = new()
        {
            Name = request.Name,
            Birthday = request.Birthday.ToDateTime()
        };

        var id = _repository.Add(pet);

        CreatePetResponse response = new() { PetId = id };
        return Task.FromResult(response);
    }

    public override Task<UpdatePetResponse> UpdatePet(UpdatePetRequest request, ServerCallContext context)
    {
        Pet pet = new()
        {
            Name = request.Name,
            Birthday = request.Birthday.ToDateTime()
        };

        _repository.Update(pet);

        return Task.FromResult(new UpdatePetResponse());
    }

    public override Task<DeletePetResponse> DeletePet(DeletePetRequest request, ServerCallContext context)
    {
        _repository.Delete(request.PetId);

        return Task.FromResult(new DeletePetResponse());
    }

    public override Task<PetResponse> GetPetById(GetPetByIdRequest request, ServerCallContext context)
    {
        var entity = _repository.GetById(request.PetId);
        PetResponse response = entity is null ? new() : Map(entity);
        return Task.FromResult(response);
    }

    public override Task<GetPetsResponse> GetPets(GetPetsRequest request, ServerCallContext context)
    {
        var entities = _repository.GetAll();
        GetPetsResponse response = new();
        if (entities.Count > 0) response.Pets.AddRange(entities.Select(e => Map(e)));
        return Task.FromResult(response);
    }

    private static PetResponse Map(Pet data) => new()
    {
        PetId = data.Id,
        Name = data.Name,
        Birthday = data.Birthday.ToTimestamp()
    };
}