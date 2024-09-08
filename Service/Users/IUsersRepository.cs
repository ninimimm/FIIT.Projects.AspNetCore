namespace Service.Users;

public interface IUsersRepository
{
    Task<bool> CheckUser(string userTag);
    Task<string> GetUserId(string userTag);
    Task AddConnectId(string userTag, string userId);
}