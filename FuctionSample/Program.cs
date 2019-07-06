using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuctionSample
{
    class Program
    {
        static void Main(string[] args)
        {

            var d = new Dog();
            d.CanDoOther();

            Console.ReadLine();

        }
    }

    public class Animal
    {
        public string Color { get; set; }
    }

    public class Mamal : Animal
    {
        public int Height { get; set; }
    }

    public class Dog : Mamal, IOther
    {
        public void CanDoOther()
        {
            Console.WriteLine("can do dog");
        }
    }

    public interface IOther
    {
        void CanDoOther();
    }

    public class OtherClass : IOther
    {
        public void CanDoOther()
        {
            Console.WriteLine("Other func");
        }
    }
}
