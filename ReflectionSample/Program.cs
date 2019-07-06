using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine(assembly.FullName);

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                Console.WriteLine("Type: " + type.Name);

                // Get all properties
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    Console.WriteLine("\t Property: " + prop.Name + " PropertyType: " + prop.PropertyType);
                }

                // Get all fileds
                var fileds = type.GetFields();
                foreach (var filed in fileds)
                {
                    Console.WriteLine("\t Field: " + filed.Name);
                }

                // Get all methods
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    Console.WriteLine("\t Method: " + method.Name);
                }
            }

            var sample = new Sample { Name = "Tharindu", Weight = 12, Age = 30 };
            var sampleType = typeof(Sample);

            var propertyName = sampleType.GetProperty("Name");
            Console.WriteLine("Name Property: " + propertyName.GetValue(sample));


            // Reflection usage
            var assembly1 = Assembly.GetExecutingAssembly();
            var types1 = assembly1.GetTypes().Where(t => t.GetCustomAttributes<MyClassAttribute>().Count() > 0);
            foreach (var type in types1)
            {
                Console.WriteLine("Attribute Class: " + type.Name);

                // get methods
                var methods1 = type.GetMethods().Where(m => m.GetCustomAttributes<MyMethodAttribute>().Count() > 0);
                foreach (var method in methods1)
                {
                    Console.WriteLine("Attribute Method: " + method.Name);
                }
            }

            Console.ReadKey();
        }
    }

    [MyClass]
    public class Sample
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public int Weight;

        [MyMethod]
        public void Do()
        {
            Console.WriteLine("my console message !");
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MyClassAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MyMethodAttribute: Attribute
    {

    }
}
