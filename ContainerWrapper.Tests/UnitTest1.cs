using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContainerWrapper.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestMethod1Async()
        {
            DockerContainerManager manager = new DockerContainerManager();
            await manager.ProvisionDockerContainerAsync("hello-world", "latest");
        }
    }
}
