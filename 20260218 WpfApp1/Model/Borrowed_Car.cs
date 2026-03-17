using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.Model
{
    internal class Borrowed_Car
    {
        public Car Car {get; set;}
        public string BorrowerName { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }

        public Borrowed_Car(Car car, string borrowerName, string startDateTime, string endDateTime)
        {
            Car = car;
            BorrowerName = borrowerName;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
    }
}
