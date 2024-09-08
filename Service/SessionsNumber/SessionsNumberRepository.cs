using Microsoft.EntityFrameworkCore;
using Models.Passport;
using Models.PassportBoard;
using Service.Passports;

namespace Service.SessionsNumber;

public class SessionsNumberRepository(ApplicationDbContext.ApplicationDbContext db, PassportsRepository passports)
    : ISessionsNumberRepository
{
    private async Task<int> GetMaxNumber(Status status)
    {
        var sessionNumbers = await db.SessionNumbers.Where(x => x.Status == status).ToListAsync();
        return sessionNumbers.Count != 0 ? sessionNumbers.Max(x => x.Number) : 0;
    }

    public async Task CreateSessionNumber(string sessionId, string name)
    {
        if (await db.SessionNumbers.FindAsync(sessionId) is not null)
            return;
        var sessionNumber = new SessionNumber(sessionId, await GetMaxNumber(Status.SendToReview) + 1, Status.SendToReview, name);
        await db.SessionNumbers.AddAsync(sessionNumber);
        await db.SaveChangesAsync();
    }

    private async Task<SessionNumber?> GetSessionNumber(string? idSession) => await db.SessionNumbers.FindAsync(idSession);

    public async Task UpdateSessionNumber(string sessionId, int number, Status status, string name)
    {
        var sessionNumber = new SessionNumber(sessionId, number, status, name);
        var sn = await GetSessionNumber(sessionId);
        var passport = await passports.GetPassport(sessionId);
        passport!.Status = status;
        await passports.UpdatePassport(passport);
        sn!.Update(sessionNumber);
        await db.SaveChangesAsync();
    }

    public async Task<List<SessionNumber>> GetAllSessionNumbers() =>
        await db.SessionNumbers.ToListAsync();
}