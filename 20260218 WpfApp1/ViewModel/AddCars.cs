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
    internal class AddCars
    {
        public string FilePath { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand AddCarsCommand { get; set; }
        public AddCars()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            AddCarsCommand = new RelayCommand(ExecuteAddCars);
        }

        public void ExecuteAddCars()
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

                    if (!AddCar.CheckDuplicates(licensePlate))
                    {
                        Car car = new Car(name, brand, age, licensePlate);
                        Cars_In.carsAvailable.Add(car);
                        MessageBox.Show($"{car.Name} has been added to the inventory successfully.");
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
            Application.Current.Windows.OfType<View.AddCars>().FirstOrDefault()?.Close();                 // ✅ Close login after
        }
    }
}
