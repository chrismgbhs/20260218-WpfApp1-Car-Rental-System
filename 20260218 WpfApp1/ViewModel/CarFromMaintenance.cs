using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class CarFromMaintenance : ObservableObject
    {
        private Maintenance _selectedCar;
        public Maintenance maintenance;

        public ICommand RemoveCarFromMaintenanceCommand { get; set; }
        public Maintenance SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));

                if (SelectedCar != null)
                {
                    maintenance = new Maintenance(SelectedCar.Car, SelectedCar.MaintenanceDetails, SelectedCar.MaintenanceWorker, SelectedCar.StartDate);
                    //MessageBox.Show($"Selected Car:\nName: {maintenance.Car.Name}\nBrand: {maintenance.Car.Brand}\nAge: {maintenance.Car.Age}\nLicense Plate: {maintenance.Car.LicensePlate}");
                    //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
                }
            }
        }

        public CarFromMaintenance()
        {
            RemoveCarFromMaintenanceCommand = new RelayCommand(RemoveCarFromMaintenance);
        }

        public void RemoveCarFromMaintenance()
        {
            Car car = new Car (maintenance.Car.Name, maintenance.Car.Brand, maintenance.Car.Age, maintenance.Car.LicensePlate);
            Cars_In.carsAvailable.Add(car);
            File_Manager file_Manager = new File_Manager("");
            foreach (var maintenanceCar in Cars_in_Maintenance.carsInMaintenance)
            {
                if (maintenanceCar.Car.LicensePlate == car.LicensePlate)
                {
                    file_Manager = new File_Manager($"{maintenanceCar.Car.LicensePlate}");
                    break;
                }
            }

            List<string> content = new List<string>();
            content.Add($"{maintenance.StartDate} | Maintenance details: {maintenance.MaintenanceDetails} | Maintenance worker: {maintenance.MaintenanceWorker} | Completed: {DateTime.Now}");

            file_Manager.Write(content);
            MessageBox.Show("Car has been returned from maintenance successfully.");
            File_Manager file_manager = new File_Manager($"{maintenance.Car.LicensePlate}.csv");
            file_manager.Write(content);
            foreach (var maintenanceCar in Cars_in_Maintenance.carsInMaintenance)
            {
                if (maintenanceCar.Car.LicensePlate == car.LicensePlate)
                {
                    Cars_in_Maintenance.carsInMaintenance.Remove(maintenanceCar);
                    MessageBox.Show("Car has been removed from maintenance list.");
                    break;
                }
            }
        }
    }
}
