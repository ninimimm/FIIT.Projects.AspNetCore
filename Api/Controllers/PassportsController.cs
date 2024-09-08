using Microsoft.AspNetCore.Mvc;
using Models.Passport;
using Service.Passports;
using Service.SessionsNumber;
using Service.Validation;

namespace Api.Controllers;

[ApiController]
[Route("api/passports")]
public class ApiController(PassportsRepository passports, SessionsNumberRepository sessionsNumber) : ControllerBase
{
    [HttpPost("update")]
    public async Task UpdatePassport([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        if (PassportValidation.CheckValidity(passport))
            await passports.UpdatePassport(passport);
    }

    [HttpPost("confirm/passport")]
    public async Task<IActionResult> ConfirmPassport([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        var correctPassport = await passports.GetPassport(passport.SessionId);
        var response = new Dictionary<string, string>
        {
            { "state", "success" },
            { "message", "Ok" }
        };
        if (passport.TelegramTag == "")
        {
            response["state"] = "error";
            response["message"] = "Имя пользователя telegram не может быть пустым";
            return Ok(response);
        }

        if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            response["state"] = "error";
            response["message"] = $"Сначала подтвердите свою личность для пользователя {passport.TelegramTag}";
            return Ok(response);
        }

        if (PassportValidation.CheckValidity(passport))
            await passports.UpdatePassport(passport);
        else
        {
            response["state"] = "error";
            response["message"] = "Не все поля паспорта валидны";
            return Ok(response);
        }

        passport.Status = Status.SendToReview;
        await sessionsNumber.CreateSessionNumber(passport.SessionId!, passport.ProjectName!);
        return Ok(response);
    }

    [HttpPost("create/passport")]
    public async Task<IActionResult> CreatePassport()
    {
        var sessionId = Guid.NewGuid().ToString();
        await passports.CreatePassport(sessionId);
        var json = new Dictionary<string, string>
        {
            { "SessionId", sessionId }
        };
        return Ok(json);
    }

    [HttpPost("get/passport")]
    public async Task<IActionResult> GetPassport([FromBody] Dictionary<string, string> request)
    {
        var sessionId = request["sessionId"];
        var passport = await passports.GetPassport(sessionId);
        return Ok(passport);
    }

    [HttpPost("get/passports")]
    public async Task<IActionResult> GetPassports([FromBody] Dictionary<string, string> request) =>
        // var sessionId = request["sessionId"];
        // var passport = await repo.GetPassport(sessionId);
        Ok(await passports.GetPassports(request["auth"]));
}