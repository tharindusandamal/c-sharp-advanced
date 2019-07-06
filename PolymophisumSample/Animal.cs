using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymophisumSample
{
    public class Animal
    {
        public virtual void Run()
        {
            Console.WriteLine("Animal is running..!");
        }
    }

    public class Dog : Animal
    {
        public override void Run()
        {
            Console.WriteLine("Dog is running...!");
            base.Run();
        }
    }
}
