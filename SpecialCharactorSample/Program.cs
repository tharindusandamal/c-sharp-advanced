using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpecialCharactorSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string value = "s";
            var v = CheckSpecialCharacters(value);
            Console.ReadLine();
        }

        private static bool CheckSpecialCharacters(string val)
        {
            bool has = false;
            char[] c = new char[] { '\"', '<', '>', '?', '\'', '~', '*', '|', ':', '\\', '/' };
            foreach (var item in c)
            {
                if (val.Contains(item))
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
    }
}
