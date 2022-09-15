using Grpc.Net.Client;
using static ClinicServiceProtos.ClinicClientService;

AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var channel = GrpcChannel.ForAddress("http://localhost:5076");


ClinicClientServiceClient client = new(channel);

var createClientResponse = client.CreateClient(new()
{
    Document = "PASS123",
    FirstName = "Василий",
    Surname = "Пупкин",
    Patronymic = "Картошкович"
});

Console.WriteLine($"Client ({createClientResponse.ClientId}) created successfully.");

var getClientsResponse = client.GetClients(new());

Console.WriteLine("Clients:");
Console.WriteLine("========\n");
foreach (var clientObj in getClientsResponse.Clients)
{
    Console.WriteLine($"{clientObj.Document} >> {clientObj.Surname} {clientObj.FirstName}");
}

Console.ReadKey();