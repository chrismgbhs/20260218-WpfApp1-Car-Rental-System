using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace _20260218_WpfApp1.ViewModel
{
    internal class AddCar
    {
        public string CarModel { get; set; }
        public string CarAge { get; set; }
        public string CarBrand { get; set; }
        public string PlateNumber { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public AddCar()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            SubmitCommand = new RelayCommand(ExecuteSubmit);
        }

        public void ExecuteSubmit()
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
            Application.Current.Windows.OfType<View.AddCar>().FirstOrDefault()?.Close();                 // ✅ Close login after
        }
    }
}
