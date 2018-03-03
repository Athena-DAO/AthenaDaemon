using ContainerManagement;
using Interfaces;
using MachineManagementAndInformation;
using QueueConsumer;
using System;
using System.IO;
using System.Threading;

namespace TemporaryFrontEnd
{
    class Program
    {
        //private static DockerManager containerManager;

        static void Main(string[] args)
        {
            //containerManager = DockerManager.Instance;
            //containerManager.InitClient("npipe://./pipe/docker_engine");
            //(string output, string error) = containerManager.RunImage("lalitadithya/sampleapp", "latest", new string[] { "-u", "url" });
            //Console.WriteLine("output is " + output);
            //Console.WriteLine("error is " + error);

            Console.WriteLine("Number of containers = " + MachineInformation.NumberOfContainersSupported);
            Console.ReadLine();

            IListener listener = new Listener(new Consumer("localhost", "task_queue1"), new MessageRecieved());

            new Thread(() =>
            {
                listener.StartListening();
            }).Start();

            //Consumer myConsumer = new Consumer("localhost", "task_queue1");
            //myConsumer.Consume(new MessageRecieved());
            Console.ReadLine();
        }

        private class MessageRecieved : IMessageRecieved
        {
            public void ProcessMessage(string message)
            {
                Console.WriteLine("Recieved " + message);
            }
        }
    }
}
