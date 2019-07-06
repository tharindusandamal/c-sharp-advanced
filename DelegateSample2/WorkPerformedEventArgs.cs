using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateSample2
{
    public class WorkPerformedEventArgs : EventArgs
    {
        public WorkPerformedEventArgs(int h, WorkType type)
        {
            Hours = h;
            WorkType = type;
        }
        public int Hours { get; set; }
        public WorkType WorkType { get; set; }
    }
}
