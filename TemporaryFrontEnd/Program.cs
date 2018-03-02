using ContainerWrapper;
using System;

namespace TemporaryFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            DockerContainerManager containerManager = new DockerContainerManager();
            containerManager.ProvisionDockerContainerAsync("lalitadithya/sampleapp", "latest").Wait();
        }
    }
}
