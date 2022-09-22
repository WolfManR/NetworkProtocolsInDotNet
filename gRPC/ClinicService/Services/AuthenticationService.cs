using ClinicService.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClinicService.Models;
using ClinicService.Utils;
using Microsoft.AspNetCore.Authorization;

namespace ClinicService.Services;

[Authorize]
public class AuthenticationService : IAuthenticationService
{
    public const string TokenSecretKey = "kYp3s6v9y/B?E(H+";

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Dictionary<string, SessionContext> _sessions = new();

    public AuthenticationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public SessionContext? GetSessionInfo(string sessionToken)
    {
        SessionContext? sessionContext;

        lock (_sessions)
        {
            _sessions.TryGetValue(sessionToken, out sessionContext);
        }

        if (sessionContext is not null) return sessionContext;

        using var scope = _serviceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ClinicContext>();

        AccountSession? session = context.AccountSessions.FirstOrDefault(s => s.Token == sessionToken);
        if (session is null) return null;
        
        sessionContext = GetSessionContext(session.Account, session);

        lock (_sessions)
        {
            _sessions[sessionContext.SessionToken] = sessionContext;
        }

        return sessionContext;
    }

    [AllowAnonymous]
    public AuthenticationResult Login(string login, string password)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ClinicContext>();

        Account? account = FindAccountByLogin(context, login);
        if (account == null) return AuthenticationResult.UserNotFound();

        if (!PasswordUtils.VerifyPassword(password, account.PasswordSalt, account.PasswordHash))
        {
            return AuthenticationResult.InvalidPassword();
        }

        AccountSession session = new()
        {
            Token = CreateSessionToken(account),
            AccountId = account.Id,
            TimeCreated = DateTime.Now,
            TimeLastRequest = DateTime.Now,
            IsClosed = false,
        };

        context.AccountSessions.Add(session);
        context.SaveChanges();

        SessionContext sessionContext = GetSessionContext(account, session);

        lock (_sessions)
        {
            _sessions[sessionContext.SessionToken] = sessionContext;
        }

        return AuthenticationResult.Success(sessionContext);
    }

    private static SessionContext GetSessionContext(Account account, AccountSession accountSession)
    {
        return new SessionContext
        {
            SessionId = accountSession.Id,
            SessionToken = accountSession.Token,
            Account = new AccountDto
            {
                Id = account.Id,
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                SecondName = account.SecondName,
                Locked = account.Locked
            }
        };
    }

    private static string CreateSessionToken(Account account)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(TokenSecretKey);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                        new Claim(ClaimTypes.Name, account.Email)}),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static Account? FindAccountByLogin(ClinicContext context, string login)
    {
        return context
            .Accounts
            .FirstOrDefault(account => account.Email == login);
    }
}