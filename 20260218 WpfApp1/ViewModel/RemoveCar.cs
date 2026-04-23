using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class RemoveCar : ObservableObject
    {
        private Car _selectedCar;
        public Car car;

        public ICommand BackCommand { get; set; }
        public ICommand RemoveCarCommand { get; set; }

        public RemoveCar()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            RemoveCarCommand = new RelayCommand(RemoveSelectedCar);
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
                .OfType<View.RemoveCar>()
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
    }
}
