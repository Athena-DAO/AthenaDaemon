﻿using ContainerManagement;
using ContainerWrapper;
using MachineManagementAndInformation;
using QueueConsumer;
using System;
using System.IO;

namespace TemporaryFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            DockerManager containerManager = DockerManager.Instance;
            containerManager.InitDockerClient("npipe://./pipe/docker_engine");
            (string output, string error) = containerManager.RunImage("lalitadithya/sampleapp", "latest", new string[] { "-u", "url" });
            Console.WriteLine("output is " + output);
            Console.WriteLine("error is " + error);

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
