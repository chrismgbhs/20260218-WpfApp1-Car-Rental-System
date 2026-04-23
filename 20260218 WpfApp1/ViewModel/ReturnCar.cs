using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class ReturnCar : ObservableObject
    {
        public static ObservableCollection<Borrowed_Car> userRentedCars = new ObservableCollection<Borrowed_Car>();

        private Borrowed_Car _selectedCar;
        public Borrowed_Car borrowedCar;
        public Car car;
        public ICommand BackCommand { get; set; }
        public ICommand ReturnCarCommand { get; set; }

        public ReturnCar()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            ReturnCarCommand = new RelayCommand(ExecuteReturnCar);
            LoadRentedCars();
        }

        public Borrowed_Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));

                if (SelectedCar != null)
                {
                    car = new Car(SelectedCar.Car.Name, SelectedCar.Car.Brand, SelectedCar.Car.Age, SelectedCar.Car.LicensePlate);
                    borrowedCar = new Borrowed_Car(car, SelectedCar.BorrowerName, SelectedCar.StartDateTime, SelectedCar.EndDateTime);
                }
            }
        }

        public void LoadRentedCars()
        {
            userRentedCars.Clear();
            foreach (var rent in Cars_Out.carsRented)
            {
                if (rent.BorrowerName == LoginViewModel.CurrentUser.Username)
                {
                    userRentedCars.Add(rent);
                }
            }
        }

        public async void ExecuteReturnCar()
        {
            if (SelectedCar != null)
            {
                Cars_In.carsAvailable.Add(SelectedCar.Car);
                Cars_Out.carsRented.Remove(SelectedCar);
                MessageBox.Show("Car has been returned successfully.");
                LoadRentedCars();
                await DatabaseManager.RefreshDatabase();
            }

            else
            {
                MessageBox.Show("Please select a car to return.");
            }
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
            Application.Current.Windows.OfType<View.ReturnCar>().FirstOrDefault()?.Close();                 // ✅ Close login after
        }
    }
}
