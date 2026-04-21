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
        public static ObservableCollection<Borrowed_Car> rentedCars = new ObservableCollection<Borrowed_Car>();
        private Car _selectedCar;
        public Car car;
        public Borrowed_Car borrowedCar;
        public ICommand BackCommand { get; set; }
        public ICommand ReturnCarCommand { get; set; }

        public ReturnCar()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            ReturnCarCommand = new RelayCommand(ExecuteReturnCar);
            LoadRentedCars();
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
                    //car = new Car(SelectedCar.Name, SelectedCar.Brand, SelectedCar.Age, SelectedCar.LicensePlate);
                    //borrowedCar = new Borrowed_Car(car, SelectedCar.BorrowerName, SelectedCar.Sta)
                    //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
                }
            }
        }

        public void LoadRentedCars()
        {
            rentedCars.Clear();
            foreach (var rent in Cars_Out.carsRented)
            {
                if (rent.BorrowerName == LoginViewModel.CurrentUser.Username)
                {
                    rentedCars.Add(rent);
                }
            }
        }

        public void ExecuteReturnCar()
        {
            Cars_In.carsAvailable.Add(SelectedCar);
            Cars_Out.carsRented.Remove(SelectedCar);
            Console.WriteLine("Car has been returned successfully.");
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
