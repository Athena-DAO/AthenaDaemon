using ContainerWrapper;
using System;

namespace TemporaryFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            DockerContainerManager containerManager = new DockerContainerManager();
            containerManager.ProvisionDockerContainerAsync("hello-world", "latest").Wait();
        }
    }
}
