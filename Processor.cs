using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Batcher;
using Entity;

namespace Processor
{
       public class Processor : aProcessor
        {
            #region [ PRIVATE VARIABLE]
            private readonly Scheduler scheduler;
            private string sessionId;
            private string serviceuri;
            //need to delete
            public string name;

            //private bool isProcessing;
            public bool IsProcessing { get; set; }

            #endregion

            #region [CONSTRUCTOR]
            /// <summary>
            /// Creating Scheduler.
            /// 1. Passing the RiverBed name and getting the Scheduler data ex: TimeBound,BatchSize and ThreashHold.
            /// </summary>
            public Processor()
            {
                scheduler = new Scheduler();
                scheduler.MyProcessor = this;
            }
            #endregion
            #region [ aProcessor IMPLEMENTAION]
            public override void AddTask(SyncEntity Entity)
            {
                scheduler.AddTask(Entity);
            }
            #endregion
            public override void ProcessData(List<Entity.SyncEntity> batch)
            {
                
            }
        }
}

           