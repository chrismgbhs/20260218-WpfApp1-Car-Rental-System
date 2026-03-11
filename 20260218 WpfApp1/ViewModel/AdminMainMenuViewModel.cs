using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class AdminMainMenuViewModel
    {
        public ICommand ViewAvailableCarsCommand { get; set; }
        public ICommand ViewRentedCarsCommand { get; set; }
        public ICommand SendReturnCarFromMaintenanceCommand { get; set; }
        public ICommand ViewCarsInMaintenanceCommand { get; set; }
        public ICommand ViewMaintenanceHistoryCommand { get; set; }
        public ICommand AddACarCommand { get; set; }
        public ICommand AddMultipleCarsViaCSVFileCommand { get; set; }
        public ICommand ExitAndSaveCommand { get; set; }


        public AdminMainMenuViewModel()
        {
            ViewAvailableCarsCommand = new RelayCommand(ExecuteViewAvailableCars);
            ViewRentedCarsCommand = new RelayCommand(ExecuteViewRentedCars);
            SendReturnCarFromMaintenanceCommand = new RelayCommand(ExecuteSendReturnCarFromMaintenance);
            ViewCarsInMaintenanceCommand = new RelayCommand(ExecuteViewCarsInMaintenance);
            ViewMaintenanceHistoryCommand = new RelayCommand(ExecuteViewMaintenanceHistory);
            AddACarCommand = new RelayCommand(ExecuteAddACar);
            AddMultipleCarsViaCSVFileCommand = new RelayCommand(ExecuteAddMultipleCarsViaCSVFile);
            ExitAndSaveCommand = new RelayCommand(ExecuteExitAndSave);
        }

        //Implement method and logic.
        private void ExecuteViewAvailableCars()
        {
            var AvailableCars = new AvailableCars();
            AvailableCars.Show();
        }
        private void ExecuteViewRentedCars()
        {
            var RentedCars = new RentedCars();
            RentedCars.Show();
        }
        private void ExecuteExitAndSave()
        {
            // Implement any necessary save logic here before exiting
            App.Current.Shutdown();
        }
        private void ExecuteSendReturnCarFromMaintenance()
        {
            var SendReturnCarFromMaintenance = new CarToMaintenance();
            SendReturnCarFromMaintenance.Show();
        }
        private void ExecuteViewCarsInMaintenance()
        {
            var ViewCarsInMaintenance = new CarsInMaintenance();
            ViewCarsInMaintenance.Show();
        }
        private void ExecuteViewMaintenanceHistory()
        {
            var ViewMaintenanceHistory = new MaintenanceHistory();
            ViewMaintenanceHistory.Show();
        }
        private void ExecuteAddACar()
        {
            var AddACar = new AddCar();
            AddACar.Show();
        }
        private void ExecuteAddMultipleCarsViaCSVFile()
        {
            var AddMultipleCarsViaCSVFile = new AddCars();
            AddMultipleCarsViaCSVFile.Show();
        }
    }
}
