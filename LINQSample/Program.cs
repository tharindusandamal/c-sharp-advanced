using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var people = new List<Person>
            {
                new Person{ FirstName = "Jhon", LastName = "Doe", Age = 25 },
                new Person{ FirstName = "Jane", LastName = "Doe", Age = 26 },
                new Person{ FirstName = "Jhon", LastName = "Williams", Age = 40 },
                new Person{ FirstName = "Samantha", LastName = "Williams", Age = 34 },
                new Person{ FirstName = "Bob", LastName = "Walters", Age = 12 }
            };

            var sample = "I enjoy writing uber-software in C#";

            Console.WriteLine("Result 1");

            var result1 = from c in sample.ToLower()
                         where c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u'
                         orderby c ascending
                         select c;

            foreach (var item in result1)
            {
                Console.WriteLine($"{item}");
            }

            Console.WriteLine("Result 2");

            var result2 = from c in sample.ToLower()
                         where c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u'
                         orderby c descending
                         group c by c;

            foreach (var item in result2)
            {
                Console.WriteLine($"Key: {item.Key},Count: {item.Count()}");
            }
            
            Console.WriteLine("Result 3");

            var result3 = from p in people
                               select p;

            foreach (var item in result3)
            {
                Console.WriteLine($"First Name: {item.FirstName} ,Last Name: {item.LastName},Age: {item.Age}");
            }

            Console.ReadLine();

        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
