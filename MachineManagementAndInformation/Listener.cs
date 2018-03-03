using Interfaces;

namespace MachineManagementAndInformation
{
    public class Listener : IListener
    {
        private IQueueConsumer consumer;
        private IMessageRecieved messageRecieved;

        public Listener(IQueueConsumer consumer, IMessageRecieved messageRecieved)
        {
            this.consumer = consumer;
            this.messageRecieved = messageRecieved;
        }

        public void StartListening()
        {
            consumer.Consume(messageRecieved);
        }

        public void StopListening()
        {
            throw new System.NotImplementedException();
        }
    }
}
