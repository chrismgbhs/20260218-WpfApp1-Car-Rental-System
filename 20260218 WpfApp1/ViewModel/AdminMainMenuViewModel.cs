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
        public ICommand AddMultipleCarsViaCSVFileCommand { get; set; }
        public ICommand ExitAndSaveCommand { get; set; }


        public AdminMainMenuViewModel()
        {
            ViewAvailableCarsCommand = new RelayCommand(ExecuteViewAvailableCars);
            ViewRentedCarsCommand = new RelayCommand(ExecuteViewRentedCars);
            SendCarToMaintenanceCommand = new RelayCommand(ExecuteSendCarToMaintenance);
            ReturnCarFromMaintenanceCommand = new RelayCommand(ExecuteReturnCarFromMaintenance);
            ViewCarsInMaintenanceCommand = new RelayCommand(ExecuteViewCarsInMaintenance);
            ViewMaintenanceHistoryCommand = new RelayCommand(ExecuteViewMaintenanceHistory);
            AddACarCommand = new RelayCommand(ExecuteAddACar);
            AddMultipleCarsViaCSVFileCommand = new RelayCommand(ExecuteAddMultipleCarsViaCSVFile);
            ExitAndSaveCommand = new RelayCommand(ExecuteExitAndSave);
        }

        //Implement method and logic.
        private void ExecuteViewAvailableCars()
        {
            Application.Current.MainWindow.Close();// Close the current main window before opening the new one
            var AvailableCars = new AvailableCars();
            AvailableCars.ShowDialog();
            Application.Current.MainWindow = AvailableCars;
        }
        private void ExecuteViewRentedCars()
        {
            Application.Current.MainWindow.Close();
            var RentedCars = new RentedCars();
            RentedCars.ShowDialog();
            Application.Current.MainWindow = RentedCars;
            
        }
        private void ExecuteExitAndSave()
        {
            // Implement any necessary save logic here before exiting
            App.Current.Shutdown();
        }
        private void ExecuteSendCarToMaintenance()
        {
            Application.Current.MainWindow.Close();
            var sendReturnCarFromMaintenance = new _20260218_WpfApp1.View.CarToMaintenance();
            sendReturnCarFromMaintenance.ShowDialog();
            Application.Current.MainWindow = sendReturnCarFromMaintenance;
            // In MainViewModel
            
        }

        private void ExecuteReturnCarFromMaintenance()
        {
            Application.Current.MainWindow.Close();
            var ReturnCarFromMaintenance = new _20260218_WpfApp1.View.CarFromMaintenance();
            ReturnCarFromMaintenance.ShowDialog();
            Application.Current.MainWindow = ReturnCarFromMaintenance;
            
        }
        private void ExecuteViewCarsInMaintenance()
        {
            Application.Current.MainWindow.Close();
            var ViewCarsInMaintenance = new CarsInMaintenance();
            ViewCarsInMaintenance.ShowDialog();
            Application.Current.MainWindow = ViewCarsInMaintenance;
        }
        private void ExecuteViewMaintenanceHistory()
        {
            Application.Current.MainWindow.Close();
            var ViewMaintenanceHistory = new MaintenanceHistory();
            ViewMaintenanceHistory.ShowDialog();
            Application.Current.MainWindow = ViewMaintenanceHistory;
            
        }
        private void ExecuteAddACar()
        {
            Application.Current.MainWindow.Close();
            var AddACar = new AddCar();
            AddACar.Show();
            Application.Current.MainWindow = AddACar;
            
        }
        private void ExecuteAddMultipleCarsViaCSVFile()
        {
            Application.Current.MainWindow.Close();
            var AddMultipleCarsViaCSVFile = new AddCars();
            AddMultipleCarsViaCSVFile.ShowDialog();
            Application.Current.MainWindow = AddMultipleCarsViaCSVFile;
            
        }
    }
}
