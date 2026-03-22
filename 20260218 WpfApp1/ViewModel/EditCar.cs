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
    internal class EditCar : ObservableObject
    {
        private string _carName;
        private string _carBrand;
        private string _carAge;
        private string _licensePlate;
        private Car _selectedCar;
        public Car car;

        public ICommand UpdateCarCommand { get; set; }
        public ICommand BackCommand { get; set; }

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

        public EditCar()
        {
            UpdateCarCommand = new RelayCommand(ExecuteUpdateCar);
            BackCommand = new RelayCommand(ExecuteBack);
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
                    //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
                }
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
                .OfType<View.EditCar>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        public void ExecuteUpdateCar()
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
                }
            }

            else
            {
                    MessageBox.Show("Please select a car to update.");
            }
        }
    }
}
