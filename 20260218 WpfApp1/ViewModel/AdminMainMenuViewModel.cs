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
    internal class AdminMainMenuViewModel
    {
        public ICommand ViewAvailableCarsCommand { get; set; }
        public ICommand ViewRentedCarsCommand { get; set; }
        public ICommand SendCarToMaintenanceCommand { get; set; }
        public ICommand ReturnCarFromMaintenanceCommand { get; set; }
        public ICommand ViewCarsInMaintenanceCommand { get; set; }
        public ICommand ViewMaintenanceHistoryCommand { get; set; }
        public ICommand AddACarCommand { get; set; }
        public ICommand RemoveACarCommand { get; set; }
        public ICommand AddMultipleCarsViaCSVFileCommand { get; set; }
        public ICommand ExitAndSaveCommand { get; set; }
        public ICommand EditACarCommand { get; set; }
        public ICommand NewWindowCommand { get; set; }


        public AdminMainMenuViewModel()
        {
            ViewAvailableCarsCommand = new RelayCommand(ExecuteViewAvailableCars);
            ViewRentedCarsCommand = new RelayCommand(ExecuteViewRentedCars);
            SendCarToMaintenanceCommand = new RelayCommand(ExecuteSendCarToMaintenance);
            ReturnCarFromMaintenanceCommand = new RelayCommand(ExecuteReturnCarFromMaintenance);
            ViewCarsInMaintenanceCommand = new RelayCommand(ExecuteViewCarsInMaintenance);
            ViewMaintenanceHistoryCommand = new RelayCommand(ExecuteViewMaintenanceHistory);
            AddACarCommand = new RelayCommand(ExecuteAddACar);
            RemoveACarCommand = new RelayCommand(ExecuteRemoveACar);
            EditACarCommand = new RelayCommand(ExecuteEditACar);
            AddMultipleCarsViaCSVFileCommand = new RelayCommand(ExecuteAddMultipleCarsViaCSVFile);
            ExitAndSaveCommand = new RelayCommand(ExecuteExitAndSave);
            NewWindowCommand = new RelayCommand(ExecuteNewWindow);
        }

        private void ExecuteNewWindow()
        {
            ViewModel.AnotherWIndow.Test = "Hello from AdminMainMenuViewModel!";
            var mainWindow = new View.AnotherWIndow();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        //Implement method and logic.
        private void ExecuteViewAvailableCars()
        {
            var mainWindow = new AvailableCars();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteViewRentedCars()
        {
            var mainWindow = new RentedCars();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after

        }
        private void ExecuteExitAndSave()
        {
            Console.WriteLine("Exiting the system. Goodbye!");
            Cars_Out.ExportCarsOutList();
            Cars_in_Maintenance.ExportMaintenancesList();
            Cars_In.ExportCarsInList();
            MessageBox.Show("Exiting the app.");
            App.Current.Shutdown();
        }
        private void ExecuteSendCarToMaintenance()
        {
            var mainWindow = new View.CarToMaintenance();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        private void ExecuteReturnCarFromMaintenance()
        {
            var mainWindow = new View.CarFromMaintenance();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after

        }
        private void ExecuteViewCarsInMaintenance()
        {
            var mainWindow = new CarsInMaintenance();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteViewMaintenanceHistory()
        {
            Window mainWindow = new View.MaintenanceHistory();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after

        }
        private void ExecuteAddACar()
        {
            var mainWindow = new View.AddCar();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after

        }

        private void ExecuteRemoveACar()
        {
            var mainWindow = new View.RemoveCar();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        private void ExecuteEditACar()
        {
            var mainWindow = new View.EditCar();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteAddMultipleCarsViaCSVFile()
        {
            Window mainWindow = new View.AddCars();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<AdminMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
    }
}
