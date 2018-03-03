using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ContainerManagement
{
    public sealed class DockerManager
    {
        private static volatile DockerManager instance;
        private static object syncRoot = new object();
        private DockerClient client;

        private DockerManager()
        {

        }

        public static DockerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new DockerManager();
                        }
                    }
                }
                return instance;
            }
        }

        public void InitDockerClient(string uri)
        {
            client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
        }

        private async System.Threading.Tasks.Task<CreateContainerResponse> CreateContainer(string parent, string containerId, string[] args)
        {
            return await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Name = containerId,
                Image = parent,
                AttachStdout = true,
                AttachStdin = false,
                ArgsEscaped = false,
                Cmd = args
            });
        }

        private async System.Threading.Tasks.Task StartContainer(string containerId)
        {
            await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters()
            {

            });
        }

        private async System.Threading.Tasks.Task StopContainer(string containerId)
        {
            await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters()
            {
                WaitBeforeKillSeconds = 1
            });
        }

        private async System.Threading.Tasks.Task RemoveContainer(string containerId)
        {
            await client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters()
            {
                Force = true
            });
        }

        private async System.Threading.Tasks.Task PullImage(string parent, string tag)
        {
            await client.Images.CreateImageAsync(new ImagesCreateParameters()
            {
                FromImage = parent,
                Tag = tag
            }, null, new Progress());
        }

        public async System.Threading.Tasks.Task<(string, string)> RunImage(string parent, string tag, string[] args)
        {
            string containerId = Guid.NewGuid().ToString();
            await PullImage(parent, tag);

            CreateContainerResponse response = await CreateContainer(parent, containerId, args);
            await StartContainer(containerId);

            string stdOut = "", stdErr = "";
            using (var stream1 = await client.Containers.AttachContainerAsync(response.ID, false, new ContainerAttachParameters()
            {
                Stream = true,
                Stderr = true,
                Stdin = false,
                Stdout = true,
                Logs = "1"
            }))
            {
                (string output, string stderr) = await stream1.ReadOutputToEndAsync(default(CancellationToken));
                stdOut += output;
                stdErr += stdErr;
            }

            await StopContainer(containerId);
            await RemoveContainer(containerId);

            return (stdOut, stdErr);
        }

        class Progress : IProgress<JSONMessage>
        {
            public void Report(JSONMessage value)
            {
            }
        }
    }
}
