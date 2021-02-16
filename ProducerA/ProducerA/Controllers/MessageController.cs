using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProducerA.Services;
using System.Threading.Tasks;

namespace ProducerA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageService _messageService;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] MessagesConfig msg)
        {
            Task.Run(() => _messageService.SendMessagesAsync(msg));
            return Ok();
        }
    }
}
