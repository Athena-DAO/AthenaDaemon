namespace Interfaces
{
    interface IQueueConsumer
    {
        void Consume(string queueName);
    }
}
