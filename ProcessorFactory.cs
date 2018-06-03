using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Processor;
namespace ProcessorFactory
{
    public class ProcessorFactory
    {
        public static aProcessor GetProcess(string ProcessorName)
        {
            return new Processor.Processor();

        }
    }
}
