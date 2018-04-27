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
            Console.WriteLine("Number of containers = " + MachineInformation.NumberOfContainersSupported);
            StartService(DockerManager.Instance, MachineInformation.NumberOfContainersSupported);
            Console.ReadLine();
        }

        private static void StartService(IContainerManager containerManager, int numberOfContainers)
        {
            containerManager.InitClient("npipe://./pipe/docker_engine");
            for (int i = 0; i < numberOfContainers; i++)
            {
                IListener listener = new Listener(new Consumer("amqp://icpodzmj:k6tphAR4wqIo8ma0kbTXZ15tozBfrzvc@eagle.rmq.cloudamqp.com/icpodzmj", "athena_task_queue"), new MessageRecieved(containerManager));
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
                string[] paramerters = message.Split("~");
                string pipelineId = paramerters[0];
                string image = paramerters[1];
                string baseUrl = "http://athena.a2hosted.com";
                Console.WriteLine(Thread.CurrentThread.Name + " Recieved " + message);
                GuiGlue(Guid.NewGuid().ToString(), image);
                //(string output, string error) = containerManager.RunImage(image, "latest", new string[] { baseUrl, pipelineId });
            }

            private void GuiGlue(string pipelineId, string image)
            {
                string basePath = @"C:\Users\lalit\Pictures\Saved Pictures\da1dafa3-7641-4b28-adc0-0bb4d332cf55_m77d9erb00ypj!App\";
                string filePath = Path.Combine(basePath, pipelineId);
                Directory.CreateDirectory(filePath);
                File.WriteAllText(Path.Combine(filePath, "name.txt"), pipelineId.Substring(0, 12));
                File.WriteAllText(Path.Combine(filePath, "image.txt"), image);
                File.WriteAllText(Path.Combine(filePath, "timestamp.txt"), DateTime.Now.ToLongDateString());
            }
        }
    }
}
