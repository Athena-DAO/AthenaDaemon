using System;

namespace MachineManagement
{
    public class SystemInformation
    {
        public static int NumberOfContainersSupported
        {
            get
            {
                return Environment.ProcessorCount;
            }
        }
    }
}
