using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var types = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.GetCustomAttributes<SampleAttribute>().Count() > 0
                        select t;

            foreach (var t in types)
            {
                Console.WriteLine(t.Name);
            }


            Console.ReadKey();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SampleAttribute : Attribute
    {
        public int Version { get; set; }
        public string Name { get; set; }
    }

    [Sample(Name ="demo", Version = 1)]
    public class Test
    {
        public int Id { get; set; }
        public void Do() { }
    }

    [Sample(Name ="sample1", Version =2)]
    public class Demo
    {
        public int Id { get; set; }
    }

    public class Test2
    {
        public int Id { get; set; }
    }
}
