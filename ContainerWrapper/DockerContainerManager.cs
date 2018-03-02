using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerWrapper
{
    public class DockerContainerManager
    {
        public async System.Threading.Tasks.Task ProvisionDockerContainerAsync(string parent, string tag)
        {
            Console.WriteLine("Pulling image " + parent + ":" + tag);
            DockerClient client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            await client.Images.CreateImageAsync(new ImagesCreateParameters()
            {
                FromImage = parent,
                Tag = tag
            }, null, new Progress());

            CreateContainerResponse response =  await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Name = "myContainerLal",
                Image = parent,
                AttachStdout = true,
                AttachStdin = false,
                ArgsEscaped = false,
                Cmd = new string[] { "-u", "http://www.google.com" }
            });

            await client.Containers.StartContainerAsync("myContainerLal", new ContainerStartParameters()
            {

            });

            var buffer = new byte[1024];

            using(var stream1 = await client.Containers.AttachContainerAsync(response.ID, false, new ContainerAttachParameters()
            {
                Stream = true,
                Stderr = true,
                Stdin = false,
                Stdout = true,
                Logs = "1"
            }))
            {
                (string output, string stderr) = await stream1.ReadOutputToEndAsync(default(CancellationToken));
                Console.WriteLine(output);
            }


            Stream stream = await client.Containers.GetContainerLogsAsync("myContainerLal", new ContainerLogsParameters()
            {
                ShowStdout = true,
                ShowStderr = true
            });

            StreamReader streamReader = new StreamReader(stream);
            Console.WriteLine(streamReader.ReadToEnd());

            await client.Containers.StopContainerAsync("myContainerLal", new ContainerStopParameters()
            {
                WaitBeforeKillSeconds = 1
            });

            await client.Containers.RemoveContainerAsync("myContainerLal", new ContainerRemoveParameters()
            {
                Force = true
            });

            //IList<ImagesListResponse> images = await client.Images.ListImagesAsync(new ImagesListParameters()
            //{
            //    All = true
            //});

            //foreach(var image in images)
            //{
            //    Console.WriteLine(image.ID);
            //}

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }

    class Progress : IProgress<JSONMessage>
    {
        public void Report(JSONMessage value)
        {
            Console.WriteLine(value.ID);
            Console.WriteLine(value.Status);
            Console.WriteLine(value.ProgressMessage);
        }
    }
}
