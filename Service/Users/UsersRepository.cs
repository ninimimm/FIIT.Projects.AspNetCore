using Models.Users;

namespace Service.Users;

public class UsersRepository(ApplicationDbContext.ApplicationDbContext db) : IUsersRepository
{
    public async Task<bool> CheckUser(string userTag)
    {
        var user = await db.ConnectIds.FindAsync(userTag);
        return user is not null;
    }

    public async Task<string> GetUserId(string userTag)
    {
        var user = await db.ConnectIds.FindAsync(userTag);
        return user!.UserTelegramId;
    }

    public async Task AddConnectId(string userTag, string userId)
    {
        if (await CheckUser(userTag))
            return;
        await db.ConnectIds.AddAsync(new ConnectId(userTag, userId));
        await db.SaveChangesAsync();
    }
}