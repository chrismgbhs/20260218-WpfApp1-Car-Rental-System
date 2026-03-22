using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
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
        public Car car;

        public ICommand SendCarToMaintenanceCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public CarToMaintenance()
        {
            SendCarToMaintenanceCommand = new RelayCommand(SendSelectedCarToMaintenance);
            BackCommand = new RelayCommand(ExecuteBack);
        }

        public void ExecuteBack()
        {
            Window mainWindow;
            if (LoginViewModel.CurrentUser.Role == "admin")
            {
                mainWindow = new AdminMainMenu();
            }

            else
            {
                mainWindow = new UserMainMenu();
            }

            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<View.CarToMaintenance>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
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
