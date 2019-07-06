using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeledateSample
{
    delegate void Operation(int number);

    class Program
    {
        static void Main(string[] args)
        {
            Operation op = Double;
            op += Truple;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Number: " + i);
                ExecuteOperator(i, op);
            }

            op = num => { Console.WriteLine(); };

            Action<int> op2 = num => { Console.WriteLine(); };
            op2(1);

            Func<int, int> func = n => { return n * n; };
            Console.WriteLine(func(3));


            Func<int, int, int> Convertor = (x, y) => x * y;







            Console.ReadKey();
        }

        public void SayHello()
        {
            Console.WriteLine("Hi, developer");
        }

        public static void Double(int n)
        {
            Console.WriteLine($"{n} x 2 = {n * 2}");
        }

        public static void Truple(int n)
        {
            Console.WriteLine($"{n} x 3 = {n * 3}");
        }

        public static void ExecuteOperator(int num, Operation operation)
        {
            operation(num);
        }
    }
}
