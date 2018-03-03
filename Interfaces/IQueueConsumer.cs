namespace Interfaces
{
    public interface IQueueConsumer
    {
        void Consume(string queueName);
    }
}
