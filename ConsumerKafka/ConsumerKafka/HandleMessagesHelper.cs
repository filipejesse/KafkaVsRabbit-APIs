using Microsoft.Extensions.Logging;
using System;

namespace ConsumerKafka
{
    public class HandleMessagesHelper : IHandleMessagesHelper
    {
        private readonly ILogger _logger;

        public HandleMessagesHelper(ILogger<IHandleMessagesHelper> logger)
        {
            _logger = logger;
        }

        public void ReceiveMessage(string msg)
        {
            _logger.LogInformation(msg);
        }
    }
}
