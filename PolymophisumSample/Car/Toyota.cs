using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymophisumSample.Car
{
    class Toyota : Car
    {
        public sealed override void Start()
        {
            Console.WriteLine("Toyota car running");
        }
    }
}
