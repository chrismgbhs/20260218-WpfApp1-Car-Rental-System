using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;
using System.Runtime.ConstrainedExecution;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Cars_In : ObservableObject
    {
        public static ObservableCollection<Car> carsAvailable = new ObservableCollection<Car>();

        public static void AddCar(Car car)
        {
            carsAvailable.Add(car);
        }

        public Cars_In()
        {
            BackCommand = new RelayCommand(ExecuteBack);
        }

        public ICommand BackCommand { get; set; }

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
                .OfType<View.AvailableCars>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        public static async Task InitializeCarsInList()
        {
            await DatabaseManager.InitializeCarsIn();
        }

        public static async Task ExportCarsToDatabase()
        {
            await DatabaseManager.ExportCarsToDatabase();
        }
    }
}
