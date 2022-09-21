using System.Net.Http.Headers;
using ClinicService.Models;
using ClinicServiceProtos;

using Grpc.Core;
using Microsoft.Net.Http.Headers;
using static ClinicServiceProtos.AuthenticateService;
using AccountDto = ClinicServiceProtos.AccountDto;
using SessionContext = ClinicServiceProtos.SessionContext;

namespace ClinicService.Services;

public class ClinicAuthenticationService : AuthenticateServiceBase
{
    private readonly IAuthenticationService _authenticationService;

    public ClinicAuthenticationService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public override Task<GetSessionResponse> GetSession(GetSessionRequest request, ServerCallContext context)
    {
        var authenticationHeader = context.RequestHeaders.FirstOrDefault(h => h.Key == HeaderNames.Authorization);
        if (!AuthenticationHeaderValue.TryParse(authenticationHeader?.Value, out var headerValue))
        {
            return Task.FromResult(new GetSessionResponse());
        }

        var sessionToken = headerValue.Parameter;
        if (sessionToken is not { Length: > 0 })
        {
            return Task.FromResult(new GetSessionResponse());
        }

        var sessionContext = _authenticationService.GetSessionInfo(sessionToken);
        if (sessionContext is null)
        {
            return Task.FromResult(new GetSessionResponse());
        }

        return Task.FromResult(new GetSessionResponse()
        {
            SessionContext = Map(sessionContext)
        });
    }

    public override Task<AuthenticationResponse> Login(AuthenticationRequest request, ServerCallContext context)
    {
        var result = _authenticationService.Login(request.UserName, request.Password);

        if (result.Status != AuthenticationStatus.Success)
        {
            return Task.FromResult(new AuthenticationResponse { Status = (int)result.Status });
        }

        context.ResponseTrailers.Add("X-Session-Token", result.SessionContext.SessionToken);

        return Task.FromResult(new AuthenticationResponse()
        {
            Status = (int)result.Status,
            SessionContext = Map(result.SessionContext)
        });
    }

    private static SessionContext Map(Models.SessionContext data)
    {
        return new SessionContext()
        {
            SessionId = data.SessionId,
            SessionToken = data.SessionToken,
            Account = new AccountDto()
            {
                AccountId = data.Account.Id,
                Email = data.Account.Email,
                FirstName = data.Account.FirstName,
                LastName = data.Account.LastName,
                SecondName = data.Account.SecondName,
                Locked = data.Account.Locked
            }
        };
    }
}