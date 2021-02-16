namespace ConsumerKafka
{
    public interface IHandleMessagesHelper
    {
        void ReceiveMessage(string msg);
    }
}