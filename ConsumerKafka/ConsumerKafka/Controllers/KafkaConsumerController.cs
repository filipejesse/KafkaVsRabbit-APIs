using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerKafka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaConsumerController : ControllerBase
    {
        private readonly ILogger<KafkaConsumerController> _logger;

        public KafkaConsumerController(ILogger<KafkaConsumerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("I'm Alive");
        }
    }
}
