using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Entity;
using ProcessorFactory;

namespace ProcessManager
{
    public class ProcessManager
    {
        private static ProcessManager CurrenInstanse = null;
        private readonly Dictionary<string, aProcessor> dictProcessor;
        private ProcessManager()
        {
            dictProcessor = new Dictionary<string, aProcessor>();
            aProcessor salesForceProcessor = ProcessorFactory.ProcessorFactory.GetProcess("");
            dictProcessor.Add("SalesForce", salesForceProcessor);

        }
        public static ProcessManager GetCurrentInstance()
        {

            if (CurrenInstanse == null)
                CurrenInstanse = new ProcessManager();
            return CurrenInstanse;

        }

        public void AddTask(SyncEntity Entity)
        {
            dictProcessor["SalesForce"].AddTask(Entity);
        }
    }
}
