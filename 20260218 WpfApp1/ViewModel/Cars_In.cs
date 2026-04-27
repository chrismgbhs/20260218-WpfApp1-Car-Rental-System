using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;
using System.Runtime.ConstrainedExecution;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Cars_In : ObservableObject
    {
        public string FilePath { get; set; }
        public static ObservableCollection<Car> carsAvailable = new ObservableCollection<Car>();
        public string CarModel { get; set; }
        public string CarAge { get; set; }
        public string CarBrand { get; set; }
        public string PlateNumber { get; set; }
        private string _carName;
        private string _carBrand;
        private string _carAge;
        private string _licensePlate;
        private Car _selectedCar;
        public Car car;
        public string MaintenanceDescription { get; set; }
        public string MaintenanceWorker { get; set; }

        public ICommand UpdateCarCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand AddCarsCommand { get; set; }
        public ICommand RemoveCarCommand { get; set; }
        public ICommand SendCarToMaintenanceCommand { get; set; }

        public Cars_In()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            SubmitCommand = new RelayCommand(ExecuteSubmit);
            AddCarsCommand = new RelayCommand(ExecuteAddCars);
            UpdateCarCommand = new RelayCommand(ExecuteUpdateCar);
            RemoveCarCommand = new RelayCommand(RemoveSelectedCar);
            SendCarToMaintenanceCommand = new RelayCommand(SendSelectedCarToMaintenance);
        }

        public string Name
        {
            get { return _carName; }
            set { _carName = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Brand
        {
            get { return _carBrand; }
            set { _carBrand = value; OnPropertyChanged(nameof(Brand)); }
        }

        public string Age
        {
            get { return _carAge; }
            set { _carAge = value; OnPropertyChanged(nameof(Age)); }
        }

        public string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; OnPropertyChanged(nameof(LicensePlate)); }
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
                    Name = SelectedCar.Name;
                    Brand = SelectedCar.Brand;
                    Age = SelectedCar.Age;
                    LicensePlate = SelectedCar.LicensePlate;
                }
            }
        }

        public async void SendSelectedCarToMaintenance()
        {
            //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
            if (SelectedCar != null)
            {
                if (MaintenanceDescription == null)
                {
                    MessageBox.Show("Please fill in all the fields before sending the car to maintenance.");
                }

                else
                {
                    MessageBox.Show($"Selected Car:\nName: {SelectedCar.Name}\nBrand: {SelectedCar.Brand}\nAge: {SelectedCar.Age}\nLicense Plate: {SelectedCar.LicensePlate}");
                    //MessageBox.Show($"{Cars_In.carsAvailable.Count} cars available");
                    Cars_in_Maintenance.carsInMaintenance.Add(new Maintenance(SelectedCar, MaintenanceDescription, MaintenanceWorker, DateTime.Now.ToString()));
                    foreach (var maintenance in Cars_in_Maintenance.carsInMaintenance)
                    {
                        if (maintenance.Car.LicensePlate == SelectedCar.LicensePlate)
                        {
                            MessageBox.Show($"{maintenance.Car.Name} sent to maintenance with description: {maintenance.MaintenanceDetails}");
                            break;
                        }
                    }
                    MessageBox.Show("Car sent to maintenance successfully!");

                    foreach (var availableCar in Cars_In.carsAvailable)
                    {
                        if (availableCar.LicensePlate == SelectedCar.LicensePlate)
                        {
                            Cars_In.carsAvailable.Remove(availableCar);
                            MessageBox.Show("Car removed from available cars list.");
                            break;
                        }
                    }

                    await DatabaseManager.RefreshDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select a car to send to maintenance.");
            }
        }

        public async void RemoveSelectedCar()
        {
            if (SelectedCar != null)
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to remove this car?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    if (Cars_In.carsAvailable.Remove(SelectedCar))
                    {
                        MessageBox.Show("Car removed successfully.");
                        await DatabaseManager.RefreshDatabase();

                    }

                    else
                    {
                        MessageBox.Show("Error: Car could not be removed.");
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a car to remove.");
            }
        }

        public async void ExecuteUpdateCar()
        {
            if (SelectedCar != null)
            {
                if (Name == null || Brand == null || Age == null || LicensePlate == null)
                {
                    MessageBox.Show("Please fill in all fields before updating the car.");
                }

                else
                {
                    SelectedCar.Name = Name;
                    SelectedCar.Brand = Brand;
                    SelectedCar.Age = Age;
                    SelectedCar.LicensePlate = LicensePlate;
                    MessageBox.Show("Car updated successfully.");
                    await DatabaseManager.RefreshDatabase();
                }
            }

            else
            {
                MessageBox.Show("Please select a car to update.");
            }
        }

        public async void ExecuteAddCars()
        {
            if (FilePath == null)
            {
                MessageBox.Show("Please select a file to import cars from.");
            }

            else
            {
                File_Manager file_Manager = new File_Manager(FilePath);
                List<string> lines = file_Manager.getLines();
                foreach (string line in lines)
                {
                    string[] carDetails = line.Split(',');
                    if (carDetails.Length == 4)
                    {
                        string name = carDetails[0].Trim();
                        string brand = carDetails[1].Trim();
                        string age = carDetails[2].Trim();
                        string licensePlate = carDetails[3].Trim();

                        if (!CheckDuplicates(licensePlate))
                        {
                            Car car = new Car(name, brand, age, licensePlate);
                            Cars_In.carsAvailable.Add(car);
                            MessageBox.Show($"{car.Name} has been added to the inventory successfully.");
                            await DatabaseManager.RefreshDatabase();
                        }

                        else
                        {
                            MessageBox.Show($"Duplicate license plate found for {licensePlate}. Car not added.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid line format. Each line must contain exactly 4 values: Name, Brand, Age, License Plate.");
                    }
                }
                MessageBox.Show("Car import process completed.");
            }
        }

        public async void ExecuteSubmit()
        {
            if (CarModel == null || CarBrand == null || CarAge == null || PlateNumber == null)
            {
                MessageBox.Show("Please fill in all fields.");
            }

            else
            {
                if (!CheckDuplicates(PlateNumber))
                {
                    Car car = new Car(CarModel, CarBrand, CarAge, PlateNumber);
                    Cars_In.AddCar(car);
                    MessageBox.Show("Car added successfully!");
                    await DatabaseManager.RefreshDatabase();
                }

                else
                {
                    MessageBox.Show("Car not added due to duplicate license plate.");
                }
            }
        }

        public static bool CheckDuplicates(string licensePlate)
        {
            bool value = false;
            foreach (Car car in Cars_In.carsAvailable)
            {
                if (car.LicensePlate == licensePlate)
                {
                    MessageBox.Show($"A car with this license plate {licensePlate} already exists.");
                    value = true;
                    break;
                }
            }
            return value;
        }

        public static void AddCar(Car car)
        {
            carsAvailable.Add(car);
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
                .OfType<View.AvailableCars>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        public static async Task InitializeCarsInList()
        {
            await DatabaseManager.InitializeCarsIn();
        }

        public static async Task ExportCarsToDatabase()
        {
            await DatabaseManager.ExportCarsToDatabase();
        }
    }
}
