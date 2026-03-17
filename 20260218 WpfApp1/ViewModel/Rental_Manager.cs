using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Rental_Manager
    {
        public static bool CheckDuplicates(string plateNumber)
        {
            bool duplicateFound = false;

            foreach (Car car in Cars_In.carsAvailable)
            {
                if (car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            foreach (Borrowed_Car borrowed_car in Cars_Out.carsRented)
            {
                if (borrowed_car.Car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            foreach (Maintenance maintenance in Cars_in_Maintenance.carsInMaintenance)
            {
                if (maintenance.Car.LicensePlate == plateNumber)
                {
                    duplicateFound = true;
                }
            }

            return duplicateFound;
        }
    }
}
