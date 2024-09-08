using Models.Passport;

namespace Service.Passports;

public interface IPassportsRepository
{
    Task<Passport> CreatePassport(string? sessionId);
    Task<bool> CheckPassport(string? idSession);
    Task<Passport?> GetPassport(string? idSession);
    Task<List<Passport>> GetPassports(string authenticatedTelegramTag);
    Task UpdatePassport(Passport passport);
}