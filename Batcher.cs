using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Abstract;
using Entity;

namespace Batcher
{
    public class Scheduler
    {
        #region [ PRIVATE VARIABLES]
        #region [Scheduler Default Properties]
        int threashhold = 10;
        int batchSize = 10;
        int timebound = 20;
        bool isRunning = false;
        #endregion

        DateTime lastTriggeredOn = DateTime.Now;
        private aProcessor myProcessor;
        readonly Thread thTaskRunner;
        readonly Queue<SyncEntity> taskQueue = new Queue<SyncEntity>();
        readonly object objLock = new object();
       // private aProcessor myProcessor;
        #endregion
         #region [PROPERTIES
        public aProcessor MyProcessor
        {
            get
            {
                return this.myProcessor;
            }
            set
            {
                this.myProcessor = value;
            }
        }

        public int BatchSize
        {
            get { return batchSize; }
            set { batchSize = value; }
        }
        public int TimeBound
        {
            get { return timebound; }
            set { timebound = value; }
        }
        public int ThreashHold
        {
            get { return threashhold; }
            set { threashhold = value; }
        }
        #endregion
        public Scheduler()
        {
            thTaskRunner = new Thread(new ThreadStart(this.RunTask));
            thTaskRunner.Start();
        }
        #region [ BATCHING LOGIC]
        private bool IsTimeBoundPassed()
        {
            TimeSpan TimeBounddiff = DateTime.Now - lastTriggeredOn;
            if (TimeBounddiff.TotalSeconds > timebound)
                return true;
            else
                return false;
        }
        private bool IsThreashHoldPass()
        {
            if (taskQueue.Count > batchSize)
                return true;
            else
            {
                TimeSpan ThreashHolddiff = DateTime.Now - lastTriggeredOn;
                if (ThreashHolddiff.TotalSeconds > threashhold)
                    return true;
                else
                    return false;
            }

        }
        private bool CanTrigger()
        {
            if (isRunning == true)
                return false;
            if (taskQueue.Count > 0)
            {
                if (IsTimeBoundPassed())
                {
                    return IsThreashHoldPass();
                }
                else
                    return false;
            }
            else
                return false;
        }
        #endregion

        public void AddTask(SyncEntity Task)
        {
            lock (objLock)
            {
                taskQueue.Enqueue(Task);
            }
        }
        private void RunTask()
        {

            while (true)
            {
                    if (this.CanTrigger())
                    {
                        isRunning = true;
                        Guid BatchGuid;
                        BatchGuid = Guid.NewGuid();
                        List<SyncEntity> Tasks = new List<SyncEntity>();
                        SyncEntity FirstTask = null;
                        lock (objLock)
                        {
                            FirstTask = taskQueue.Dequeue();
                            FirstTask.BatchID = BatchGuid;
                            Tasks.Add(FirstTask);

                            while (taskQueue.Count > 0 && Tasks.Count <= batchSize && taskQueue.Peek().CompareTo(FirstTask) == 1)
                            {
                                SyncEntity QueueTask = taskQueue.Dequeue();
                                QueueTask.BatchID = BatchGuid;
                                Tasks.Add(QueueTask);
                            }
                        }
                        myProcessor.ProcessData(Tasks);
                        lastTriggeredOn = DateTime.Now;
                        Thread.Sleep(200);
                        isRunning = false;
                    }
                    Thread.Sleep(200);
                }
            }
    }
}
    