using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Cars_Out : ObservableObject
    {
        public static ObservableCollection<Borrowed_Car> carsRented = new ObservableCollection<Borrowed_Car>();

        public ICommand BackCommand { get; set; }

        public Cars_Out()
        {
            BackCommand = new RelayCommand(ExecuteBack);
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
                .OfType<View.RentedCars>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        public static async Task InitializeCarsOutList()
        {
            await DatabaseManager.InitializeCarsOut();
        }

        public static async Task ExportCarsOutList()
        {
            await DatabaseManager.ExportCarsOut();
        }
    }
}
