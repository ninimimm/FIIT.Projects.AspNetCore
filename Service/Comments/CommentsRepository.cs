using Microsoft.EntityFrameworkCore;
using Models.Passport;

namespace Service.Comments;

public class CommentsRepository(ApplicationDbContext.ApplicationDbContext db) : ICommentsRepository
{
    public async Task<int> CreateComment(string sessionId, string name, int start, int end, string text)
    {
        var newId = 1;
        if (db.Comments.Any())
            newId += await db.Comments.MaxAsync(x => x.Id);
        var comment = new Comment(sessionId, name, start, end, text, newId);
        await db.Comments.AddAsync(comment);
        await db.SaveChangesAsync();
        return newId;
    }

    public async Task UpdateComment(int id, string text)
    {
        var comment = await db.Comments.FindAsync(id);
        if (comment is null) return;
        comment.Update(text);
        await db.SaveChangesAsync();
    }

    public async Task DeleteComment(int id)
    {
        var comment = await GetComment(id);
        db.Comments.Remove(comment!);
        await db.SaveChangesAsync();
    }

    private async Task<Comment?> GetComment(int id) =>
        await db.Comments.FindAsync(id);

    public async Task<List<Comment>> GetCommentsBySessionId(string sessionId) =>
        await db.Comments.Where(c => c.SessionId == sessionId).ToListAsync();
}