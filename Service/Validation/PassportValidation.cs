using System.Text.RegularExpressions;
using Models.Passport;

namespace Service.Validation;

public static partial class PassportValidation
{
    public static bool CheckValidity(Passport passport)
    {
        if (passport.Name is not null && (passport.PhoneNumber == "" || !PhoneRegex().IsMatch(passport.PhoneNumber!) ||
            passport.Email == "" || !EmailRegex().IsMatch(passport.Email!) ||
            passport.TelegramTag == "" || !TelegramTagRegex().IsMatch(passport.TelegramTag!) ||
            passport.TelegramTag!.Length > 33 ||
            passport.Surname == "" || passport.Surname!.Length > 50 || !SurnameRegex().IsMatch(passport.Surname) ||
            passport.Name == "" || passport.Name!.Length > 50 || !NameRegex().IsMatch(passport.Name)))
            return false;
        if (passport.MeetingLocation is not null && (passport.MeetingLocation == "" || passport.MeetingLocation!.Length > 100 ||
            !MeetingLocationRegex().IsMatch(passport.MeetingLocation) ||
            passport.CopiesNumber < 1 || passport.CopiesNumber > 5))
            return false;
        if (passport.AcceptanceCriteria is not null && (passport.AcceptanceCriteria == "" || passport.AcceptanceCriteria!.Length > 100000 ||
            passport.Result == "" || passport.Result!.Length > 100000 ||
            passport.Goal == "" || passport.Goal!.Length > 100000))
            return false;
        if (passport.ProjectDescription is not null && (passport.ProjectDescription == "" || passport.ProjectDescription!.Length > 100000 ||
            passport.ProjectName == "" || passport.ProjectName!.Length > 100 ||
            passport.OrdererName == "" || passport.OrdererName!.Length > 100))
            return false;
        return passport.SessionId!.Length == 36;
    }

    [GeneratedRegex(@"^[A-Za-zА-Яа-я]+$")]
    public static partial Regex NameRegex();

    [GeneratedRegex(@"^[а-яa-zA-ZА-Я]+$")]
    public static partial Regex SurnameRegex();

    [GeneratedRegex(@"^\@[0-9a-zA-Z_]+$")]
    public static partial Regex TelegramTagRegex();

    [GeneratedRegex(@"^\+\d \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
    public static partial Regex PhoneRegex();

    [GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    public static partial Regex EmailRegex();

    [GeneratedRegex(@"^[а-яa-zA-ZА-Я0-9, .:;'""/\\()]+$")]
    public static partial Regex MeetingLocationRegex();
}