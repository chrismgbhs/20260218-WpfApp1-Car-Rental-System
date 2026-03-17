using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.Model
{
    internal class Car
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Age { get; set; }
        public string LicensePlate { get; set; }

        public Car(string name, string brand, string age, string licensePlate)
        {
            Name = name;
            Brand = brand;
            Age = age;
            LicensePlate = licensePlate;
        }
    }
}
