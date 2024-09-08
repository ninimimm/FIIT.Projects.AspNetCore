using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Passport;

[Table("comments")]
public class Comment(string sessionId, string fieldName, int startIndex, int endIndex, string textComment, int id = 0)
{
    [Key] [Column("id")] public int Id { get; set; } = id;

    [Required] [Column("session_id")] public string SessionId { get; set; } = sessionId;

    [Column("field_name")] public string FieldName { get; set; } = fieldName;

    [Column("start_index")] public int StartIndex { get; set; } = startIndex;

    [Column("end_index")] public int EndIndex { get; set; } = endIndex;

    [Column("text_comment")] public string TextComment { get; set; } = textComment;

    public void Update(string text)
    {
        TextComment = text;
    }
}