using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProducerA.Services
{
    public interface IMessageService
    {
        Task SendMessagesAsync(MessagesConfig msg);
    }
}