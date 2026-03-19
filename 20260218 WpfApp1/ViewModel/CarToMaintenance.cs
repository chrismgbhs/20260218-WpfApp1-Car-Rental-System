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
        public string MaintenanceWorker = LoginViewModel.CurrentUser.Username;
        private Car _selectedCar;
        public Car car;

        public ICommand SendCarToMaintenanceCommand { get; set; }
        public Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));

                if (SelectedCar != null)
                {
                    car = new Car(SelectedCar.Name, SelectedCar.Brand, SelectedCar.Age, SelectedCar.LicensePlate);
                    //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
                }
            }
        }

        public CarToMaintenance()
        {
            SendCarToMaintenanceCommand = new RelayCommand(SendSelectedCarToMaintenance);
        }

        public void SendSelectedCarToMaintenance()
        {
            //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
            if (car != null)
            {
                if (MaintenanceDescription == null)
                {
                    MessageBox.Show("Please fill in all the fields before sending the car to maintenance.");
                }

                else
                {
                    MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
                    //MessageBox.Show($"{Cars_In.carsAvailable.Count} cars available");
                    Cars_in_Maintenance.carsInMaintenance.Add(new Maintenance(car, MaintenanceDescription, MaintenanceWorker, DateTime.Now.ToString()));

                    foreach (var maintenance in Cars_in_Maintenance.carsInMaintenance)
                    {
                        if (maintenance.Car.LicensePlate == car.LicensePlate)
                        {
                            MessageBox.Show($"{maintenance.Car.Name} sent to maintenance with description: {maintenance.MaintenanceDetails}");
                            break;
                        }
                    }
                    MessageBox.Show("Car sent to maintenance successfully!");
                    
                    foreach (var availableCar in Cars_In.carsAvailable)
                    {
                        if (availableCar.LicensePlate == car.LicensePlate)
                        {
                            Cars_In.carsAvailable.Remove(availableCar);
                            MessageBox.Show("Car removed from available cars list.");
                            break;
                        }
                    }
                }
            }
        }
    }
}
