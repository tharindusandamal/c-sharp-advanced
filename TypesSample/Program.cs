using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesSample
{
    class Program
    {
        static void Main()
        {
            House h = new House
            {
                Name = "Broadway"
            };

            // Cast a House to an Apartment.
            Apartment a = (Apartment)h;

            // Apartment was converted from House.
            Console.WriteLine(a.Name);


            // Increment widget twice.
            Widget w = new Widget();
            w++;
            Console.WriteLine(w._value);
            w++;
            Console.WriteLine(w._value);

            // Create another widget.
            Widget g = new Widget();
            g++;
            Console.WriteLine(g._value);

            // Add two widgets.
            Widget t = w + g;
            Console.WriteLine(t._value);

            Role role1 = "RoleName";

            Role role = new Role();
            role.Name = "RoleName";

            Console.ReadKey();
        }
    }

    class Apartment
    {
        public string Name { get; set; }
        public static explicit operator House(Apartment a)
        {
            return new House() { Name = a.Name };
        }
    }

    class House
    {
        public string Name { get; set; }
        public static explicit operator Apartment(House h)
        {
            return new Apartment() { Name = h.Name };
        }
    }

    class Widget
    {
        public int _value;

        public static Widget operator +(Widget a, Widget b)
        {
            // Add two Widgets together.
            // ... Add the two int values and return a new Widget.
            Widget widget = new Widget
            {
                _value = a._value + b._value
            };
            return widget;
        }

        public static Widget operator ++(Widget w)
        {
            // Increment this widget.
            w._value++;
            return w;
        }
    }

    public class Role
    {
        public string Name { get; set; }

        public static implicit operator Role(string roleName)
        {
            return new Role() { Name = roleName };
        }

        public override string ToString()
        {
            return base.ToString();
        }

        //public static explicit operator Role(string roleName)
        //{
        //    return new Role() { Name = roleName };
        //}
    }
}
