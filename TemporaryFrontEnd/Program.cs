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

        static void Main(string[] args)
        {
            int numberOfContainers = 2;
            Console.WriteLine("Number of containers = " + MachineInformation.NumberOfContainersSupported);
            StartService(DockerManager.Instance, numberOfContainers);
            Console.ReadLine();
        }

        private static void StartService(IContainerManager containerManager, int numberOfContainers)
        {
            containerManager.InitClient("npipe://./pipe/docker_engine");
            for (int i = 0; i < numberOfContainers; i++)
            {
                IListener listener = new Listener(new Consumer("localhost", "task_queue1"), new MessageRecieved(containerManager));
                new Thread(() =>
                {
                    listener.StartListening();
                }).Start();
            }
        }

        private class MessageRecieved : IMessageRecieved
        {
            IContainerManager containerManager;

            public MessageRecieved(IContainerManager containerManager)
            {
                this.containerManager = containerManager;
            }

            public void ProcessMessage(string message)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " Recieved " + message);
                (string output, string error) = containerManager.RunImage("lalitadithya/sampleapp", "latest", new string[] { "-u", "url" });
                Console.WriteLine(Thread.CurrentThread.Name + " output is " + output);
                Console.WriteLine(Thread.CurrentThread.Name + " error is " + error);
            }
        }
    }
}
