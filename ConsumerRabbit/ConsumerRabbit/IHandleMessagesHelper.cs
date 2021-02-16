namespace ConsumerRabbit
{
    public interface IHandleMessagesHelper
    {
        void ReceiveMessage(string msg);
    }
}