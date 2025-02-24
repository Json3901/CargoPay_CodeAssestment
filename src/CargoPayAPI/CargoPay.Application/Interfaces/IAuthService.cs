namespace CargoPay.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string username, string password);
    Task<string> GetSession();
}