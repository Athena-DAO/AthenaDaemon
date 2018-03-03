using System;
using System.Collections.Generic;
using System.Text;

namespace MachineManagementAndInformation
{
    public class MachineInformation
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
