using ContainerWrapper;
using MachineManagementAndInformation;
using QueueConsumer;
using System;

namespace TemporaryFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            //DockerContainerManager containerManager = new DockerContainerManager();
            //containerManager.ProvisionDockerContainerAsync("lalitadithya/sampleapp", "latest").Wait();

            Console.WriteLine("Number of containers = " + MachineInformation.NumberOfContainersSupported);

            Console.ReadLine();

            Consumer myConsumer = new Consumer("localhost", "task_queue1", MyCallback);
            myConsumer.Consume("task_queue1");

            Console.ReadLine();
        }

        private static void MyCallback(string message)
        {
            Console.WriteLine("Recieved " + message);
        }
    }
}
