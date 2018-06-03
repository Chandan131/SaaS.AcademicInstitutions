using System;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity;

namespace Abstract
{
    public abstract class aProcessor
    {
        /// This function will have implementation of processing data for respective processor.
        /// </summary>
        /// <param name="tasks"></param>
        public abstract void ProcessData(List<SyncEntity> batch);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="entity"></param>
        public abstract void AddTask(SyncEntity Entity);

    }
}

