using Microsoft.AspNetCore.Mvc;
using Models.Passport;
using Service.Passports;
using Service.Users;
using Service.Validation;

namespace Api.Controllers;

[ApiController]
[Route("api/authenticate")]
public class AuthenticationController(PassportsRepository passports, UsersRepository users, TelegramBot.TelegramBot botTools)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        var response = new Dictionary<string, string>();
        var correctPassport = await passports.GetPassport(passport.SessionId);
        if (!await users.CheckUser(passport.TelegramTag!))
        {
            response["state"] = "error";
            response["message"] = $"Наш бот ждет команды /start от пользователя {passport.TelegramTag}";
        }
        else if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            await botTools.SendButton(await users.GetUserId(passport.TelegramTag!), passport.SessionId!);
            response["state"] = "error";
            response["message"] = $"{passport.TelegramTag} нажмите \"Подтвердить\"";
        }
        else
        {
            response["state"] = "success";
            response["message"] = "Теперь всё готово к отправке проекта";
        }

        if (PassportValidation.CheckValidity(passport))
            await passports.UpdatePassport(passport);
        return Ok(response);
    }
}