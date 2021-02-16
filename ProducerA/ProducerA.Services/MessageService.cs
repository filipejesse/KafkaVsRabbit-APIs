using Confluent.Kafka;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProducerA.Services
{
    public class MessageService : IMessageService
    {

        private readonly IBus _bus;
        private readonly ILogger _logger;
        private readonly IProducer<Null, string> _producer;

        public MessageService(ILogger<IMessageService> logger, IBus bus, IProducer<Null, string> producer)
        {
            _bus = bus;
            _logger = logger;
            _producer = producer;
        }

        const string queueName = "rabbitQueue";
        const string topicName = "kafkaTopic";

        public async Task SendMessagesAsync(MessagesConfig msg)
        {
            var messages = CreateMessages(msg.MessagesCount);

            if(msg.QueueType == QueueType.RabbitMQ)
                await SendToRabbitQueue(msg, messages);
            else
                await SendToKafkaTopic(msg, messages);
        }

        private async Task SendToKafkaTopic(MessagesConfig msg, IEnumerable<string> messages)
        {
            if (msg.UseParallelism)
            {
                Parallel.ForEach(messages,
                    parallelOptions: GetParallellismConfigs(msg),
                        async x =>
                        {
                            await _producer.ProduceAsync(
                            topicName,
                            new Message<Null, string>
                            { Value = x });
                        }
                    );
            } 
            else
            {
                foreach (var message in messages)
                {
                    await _producer.ProduceAsync(
                            topicName,
                            new Message<Null, string>
                            { Value = message });
                }
            }
        }

        private async Task SendToRabbitQueue(MessagesConfig msg, IEnumerable<string> messages)
        {
            if (msg.UseParallelism)
            {
                Parallel.ForEach(messages,
                    parallelOptions: GetParallellismConfigs(msg),
                        async x =>
                        {
                            await _bus.SendReceive.SendAsync(queueName, x);
                        }
                    );
            }
            else
            {
                foreach (var message in messages)
                {
                    await _bus.SendReceive.SendAsync(queueName, message);
                }
            }
        }

        private static ParallelOptions GetParallellismConfigs(MessagesConfig msg)
        {
            return new ParallelOptions { MaxDegreeOfParallelism = msg.ParallelismLimit ?? 2 };
        }

        private IEnumerable<string> CreateMessages(int messagesCount)
        {
            var text = new List<string>();

            for (var i = 0; i < messagesCount; i++)
                text.Add($"{{ \"Text\": \"Mensagem enviada via api para validar o funcionamento das mensagerias\", \"Index\": {i}}}");

            return text;
        }




    }
}
