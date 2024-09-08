using Models.PassportBoard;

namespace Service.SessionsNumber;

public interface ISessionsNumberRepository
{
    Task CreateSessionNumber(string sessionId, string name);
    Task<List<SessionNumber>> GetAllSessionNumbers();

}