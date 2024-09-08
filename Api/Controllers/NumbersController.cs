using Microsoft.AspNetCore.Mvc;
using Models.Passport;
using Service.SessionsNumber;

namespace Api.Controllers;

[ApiController]
[Route("api/numbers")]
public class NumbersController(SessionsNumberRepository sessionsNumbers) : ControllerBase
{
    [HttpPost("update")]
    public async Task UpdateNumber([FromBody] Dictionary<string, Dictionary<string, string>> request)
    {
        foreach (var sessionNumber in request)
            await sessionsNumbers.UpdateSessionNumber(sessionNumber.Key, int.Parse(sessionNumber.Value["number"]),
                (Status)int.Parse(sessionNumber.Value["status"]), sessionNumber.Value["name"]);
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetNumber()
    {
        var sessionNumbers = await sessionsNumbers.GetAllSessionNumbers();
        return Ok(sessionNumbers);
    }
}