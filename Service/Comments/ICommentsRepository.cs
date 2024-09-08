using Models.Passport;

namespace Service.Comments;

public interface ICommentsRepository
{
    Task<int> CreateComment(string sessionId, string name, int start, int end, string text);
    Task UpdateComment(int id, string text);
    Task DeleteComment(int id);
    Task<List<Comment>> GetCommentsBySessionId(string sessionId);
}