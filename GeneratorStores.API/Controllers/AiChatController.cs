using GeneratorStores.DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiChatController : ControllerBase
    {
        private readonly ChatGptService _chatService;

        public AiChatController(ChatGptService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            var response = await _chatService.AskAsync(request.Question);
            return Ok(new { answer = response });
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }

}
