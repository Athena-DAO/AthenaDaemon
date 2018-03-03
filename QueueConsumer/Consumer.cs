using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QueueConsumer
{
    public delegate void MessageRecievedCallback(string s);

    public class Consumer
    {
        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;
        MessageRecievedCallback messageRecievedCallback;

        private void InitializeConnectionFactory(string hostName)
        {
            connectionFactory = new ConnectionFactory()
            {
                HostName = hostName
            };
        }

        private void CreateConnection()
        {
            connection = connectionFactory.CreateConnection();
        }

        private void SetUpChannel(string queueName)
        {
            channel = connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.BasicQos(0, 1, false);
        }

        private void SetUpConsumer()
        {
            consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            messageRecievedCallback.Invoke(Encoding.UTF8.GetString(e.Body));
            channel.BasicAck(e.DeliveryTag, false);
            autoResetEvent.Set();
        }

        public void Consume(string queueName)
        {
            channel.BasicConsume(queueName, false, consumer);
            while (true)
            {
                autoResetEvent.WaitOne();
            }
        }

        public Consumer(string hostName, string queueName, MessageRecievedCallback messageRecievedCallback)
        {
            this.messageRecievedCallback = messageRecievedCallback;

            InitializeConnectionFactory(hostName);

            CreateConnection();

            SetUpChannel(queueName);

            SetUpConsumer();
        }
    }
}
