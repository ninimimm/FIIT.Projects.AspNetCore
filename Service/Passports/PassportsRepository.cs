using Microsoft.EntityFrameworkCore;
using Models.Passport;

namespace Service.Passports;

public class PassportsRepository(ApplicationDbContext.ApplicationDbContext db) : IPassportsRepository
{
    public async Task<Passport> CreatePassport(string? sessionId)
    {
        var passport = new Passport(sessionId);
        await db.Passports.AddAsync(passport);
        await db.SaveChangesAsync();
        return passport;
    }

    public async Task<bool> CheckPassport(string? idSession)
    {
        var passport = await db.Passports.FindAsync(idSession);
        return passport is not null;
    }

    public async Task<Passport?> GetPassport(string? idSession) => await db.Passports.FindAsync(idSession);

    public async Task<List<Passport>> GetPassports(string authenticatedTelegramTag) =>
        await db.Passports.Where(x => x.AuthenticatedTelegramTag == authenticatedTelegramTag).ToListAsync();

    public async Task UpdatePassport(Passport passport)
    {
        if (!await CheckPassport(passport.SessionId!))
            return;
        var ps = await GetPassport(passport.SessionId!);
        ps!.Update(passport);
        await db.SaveChangesAsync();
    }
}