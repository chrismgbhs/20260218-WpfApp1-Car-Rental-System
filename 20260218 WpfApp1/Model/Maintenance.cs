using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.Model
{
    internal class Maintenance
    {
        public Car Car {get; set; }
        public string MaintenanceDetails { get; set; }
        public string MaintenanceWorker { get; set; }
        public string StartDate { get; set; }

        public Maintenance(Car car, string maintenanceDetails, string maintenanceWorker, string startDate)
        {
            Car = car;
            MaintenanceDetails = maintenanceDetails;
            MaintenanceWorker = maintenanceWorker;
            StartDate = startDate;
        }
    }
}
