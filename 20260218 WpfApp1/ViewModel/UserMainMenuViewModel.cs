using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using _20260218_WpfApp1.View;

namespace _20260218_WpfApp1.ViewModel
{
    internal class UserMainMenuViewModel
    {
        public ICommand ViewAvailableCarsCommand { get; set; }
        public ICommand ViewRentedCarsCommand { get; set; }
        public ICommand RentACarCommand { get; set; }
        public ICommand ReturnACarCommand { get; set; }
        public ICommand ExitAndSaveCommand { get; set; }

        public UserMainMenuViewModel()
        {
            ViewAvailableCarsCommand = new RelayCommand(ExecuteViewAvailableCars);
            ViewRentedCarsCommand = new RelayCommand(ExecuteViewRentedCars);
            RentACarCommand = new RelayCommand(ExecuteRentACar);
            ReturnACarCommand = new RelayCommand(ExecuteReturnACar);
            ExitAndSaveCommand = new RelayCommand(ExecuteExitAndSave);
        }

        //Implement method and logic.
        private void ExecuteViewAvailableCars()
        {
            var mainWindow = new AvailableCars();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<UserMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteViewRentedCars()
        {
            var mainWindow = new RentedCars();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<UserMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteRentACar()
        {
            Window mainWindow = new View.RentCar();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<UserMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteReturnACar()
        {
            Window mainWindow = new View.ReturnCar();
            Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
            mainWindow.Show();                           // ✅ Non-blocking
            Application.Current.Windows
                .OfType<UserMainMenu>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }
        private void ExecuteExitAndSave()
        {
            // Implement any necessary save logic here before exiting
            App.Current.Shutdown();
        }
    }
}
