using Microsoft.AspNetCore.Mvc;
using Service.Comments;

namespace Api.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentsController(CommentsRepository comments) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateComment([FromBody] Dictionary<string, string> request) =>
        Ok(await comments.CreateComment(request["sessionId"], request["fieldName"],
            int.Parse(request["start"]), int.Parse(request["end"]), request["text"]));

    [HttpPost("get")]
    public async Task<IActionResult> GetComments([FromBody] Dictionary<string, string> request) =>
        Ok(await comments.GetCommentsBySessionId(request["sessionId"]));

    [HttpPost("update")]
    public async Task<IActionResult> UpdateComment([FromBody] Dictionary<string, string> request)
    {
        await comments.UpdateComment(int.Parse(request["id"]), request["text"]);
        return Ok();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteComment([FromBody] Dictionary<string, string> request)
    {
        await comments.DeleteComment(int.Parse(request["id"]));
        return Ok();
    }
}