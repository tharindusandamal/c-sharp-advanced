using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DelegateSample2
{
    //public delegate void WorkPerformHandler(object sender, WorkPerformedEventArgs e);

    public class Worker
    {
        public event EventHandler<WorkPerformedEventArgs> WorkPerformed;
        public event EventHandler WorkCompleted;

        public virtual void DoWork(int hours, WorkType type)
        {
            for (int i = 0; i < hours; i++)
            {
                Thread.Sleep(1000);
                OnWorkPerformed(i + 1, type);
            }
            //raise
            OnWorkCompleted();
        }

        protected virtual void OnWorkPerformed(int hours, WorkType type)
        {
            WorkPerformed?.Invoke(hours, new WorkPerformedEventArgs(hours, type));
        }

        protected virtual void OnWorkCompleted()
        {
            WorkCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
