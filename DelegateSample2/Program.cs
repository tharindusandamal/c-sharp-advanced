using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateSample2
{
    public delegate void WorkPerformHandler(object sender, WorkPerformedEventArgs e);

    class Program
    {
        static void Main(string[] args)
        {
            //WorkPerformHandler del1 = new WorkPerformHandler(WorkPerformd1);
            //WorkPerformHandler del2 = new WorkPerformHandler(WorkPerformd2);

            //del1 += del2;

            //DoWork(del1, 10, WorkType.GoToMeeting);

            //DoWork(del2, 10, WorkType.GoToMeeting);

            var worker = new Worker();
            worker.WorkPerformed += WorkPerformed;
            worker.WorkCompleted += WorkPerformedCompleted;
            worker.DoWork(8, WorkType.GoToMeeting);

            Console.ReadKey();
        }

        static void WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            Console.WriteLine("Hours "+ e.Hours + " " + e.WorkType);
        }

        static void WorkPerformedCompleted(object sender, EventArgs args)
        {
            Console.WriteLine("Work completed.!");
        }

        //public static void DoWork(WorkPerformHandler handler, int h, WorkType type)
        //{
        //    //handler(h, type);
        //}

        //public static void WorkPerformd1(object sender, WorkPerformedEventArgs e)
        //{
        //    Console.WriteLine("Work performd 1 called " + sender.ToString());
        //}

        //public static void WorkPerformd2(object sender, WorkPerformedEventArgs e)
        //{
        //    Console.WriteLine("Work performd 2 called " + sender.ToString());
        //}
    }

    public enum WorkType
    {
        GoToMeeting,
        PlayGolf,
        WriteDocument
    }
}
