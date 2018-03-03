using System;
using System.Collections.Generic;
using System.Text;

namespace MachineManagement
{
    public class MachineInformation
    {
        public static int NumberOfConatinersSupported
        {
            get
            {
                return Environment.ProcessorCount;
            }
        }
    }
}
