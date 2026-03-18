using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class CarToMaintenance : ObservableObject
    {
        public string MaintenanceDescription { get; set; }
        public string MaintenanceWorker { get; set; }
        private Car _selectedCar;
        public Car Car { get; set; }

        public ICommand SendSelectedCarToMaintenanceCommand { get; set; }
        public Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));

                if (SelectedCar != null)
                {
                    Car.Name = SelectedCar.Name;
                    Car.Brand = SelectedCar.Brand;
                    Car.Age = SelectedCar.Age;
                    Car.LicensePlate = SelectedCar.LicensePlate;
                }
            }
        }

        public CarToMaintenance()
        {
            SendSelectedCarToMaintenanceCommand = new RelayCommand(SendSelectedCarToMaintenance);
        }

        public void SendSelectedCarToMaintenance()
        {
            if (Car != null)
            {
                if (MaintenanceDescription == null)
                {
                    MessageBox.Show("Please fill in all the fields before sending the car to maintenance.");
                }

                else
                {
                    Cars_in_Maintenance.carsInMaintenance.Add(new Maintenance(Car, MaintenanceDescription, MaintenanceWorker, DateTime.Now.ToString()));
                    MessageBox.Show("Car sent to maintenance successfully!");
                    Cars_In.carsAvailable.Remove(Car);
                }
            }
        }
    }
}
