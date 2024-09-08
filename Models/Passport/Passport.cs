using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Passport;

public enum Status
{
    Prepare,
    SendToReview,
    Reviewing,
    Reviewed,
    SendToCommands,
    Accept,
    Reject
}

[Table("passports")]
public class Passport
{
    [Key]
    [Required]
    [Column("session_id")]
    public string? SessionId { get; set; }

    [Column("orderer_name")] public string? OrdererName { get; set; }

    [Column("project_name")] public string? ProjectName { get; set; }

    [Column("project_description")] public string? ProjectDescription { get; set; }

    [Column("goal")] public string? Goal { get; set; }

    [Column("result")] public string? Result { get; set; }

    [Column("error_message")] public string? AcceptanceCriteria { get; set; }

    [Column("copies_number")] public int CopiesNumber { get; set; } = 1;

    [Column("meeting_location")] public string? MeetingLocation { get; set; }

    [Column("name")] public string? Name { get; set; }

    [Column("surname")] public string? Surname { get; set; }

    [Column("telegram_tag")] public string? TelegramTag { get; set; }

    [Column("authenticated_telegram_tag")] public string? AuthenticatedTelegramTag { get; set; }

    [Column("email")] public string? Email { get; set; }

    [Column("phone_number")] public string? PhoneNumber { get; set; }

    [Column("status")]
    [DefaultValue(Status.Prepare)]
    public Status Status { get; set; }

    public Passport()
    {
    }

    public Passport(string? sessionId)
    {
        SessionId = sessionId;
    }

    public void Update(Passport passport)
    {
        OrdererName = passport.OrdererName ?? OrdererName;
        ProjectName = passport.ProjectName ?? ProjectName;
        ProjectDescription = passport.ProjectDescription ?? ProjectDescription;
        Goal = passport.Goal ?? Goal;
        Result = passport.Result ?? Result;
        AcceptanceCriteria = passport.AcceptanceCriteria ?? AcceptanceCriteria;
        if (passport.CopiesNumber > 1)
            CopiesNumber = passport.CopiesNumber;
        MeetingLocation = passport.MeetingLocation ?? MeetingLocation;
        Name = passport.Name ?? Name;
        Surname = passport.Surname ?? Surname;
        TelegramTag = passport.TelegramTag ?? TelegramTag;
        AuthenticatedTelegramTag = passport.AuthenticatedTelegramTag ?? AuthenticatedTelegramTag;
        Email = passport.Email ?? Email;
        PhoneNumber = passport.PhoneNumber ?? PhoneNumber;
        Status = passport.Status;
    }

    #region Reflection

    public Passport UpdateByDictionary(Dictionary<string, string> properties)
    {
        var passportProperties = GetType().GetProperties();
        foreach (var passportProperty in passportProperties)
        {
            if (!properties.TryGetValue(passportProperty.Name, out var value))
                continue;
            passportProperty.SetValue(this, passportProperty.Name != "CopiesNumber" ? value : int.Parse(value));
        }

        return this;
    }

    #endregion
}