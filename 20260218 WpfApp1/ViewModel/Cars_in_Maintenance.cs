using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Cars_in_Maintenance
    {
        public static ObservableCollection<Maintenance> carsInMaintenance = new ObservableCollection<Maintenance>();

        public ICommand BackCommand { get; set; }

        public Cars_in_Maintenance()
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
                .OfType<View.CarsInMaintenance>()
                .FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        public static void AddMaintenance(Maintenance maintenance)
        {
            carsInMaintenance.Add(maintenance);
        } 

        public static async Task InitializeMaintenancesList()
        {
            await DatabaseManager.InitializeMaintenances();
        }

        public static async Task ExportMaintenancesList()
        {
            await DatabaseManager.ExportMaintenances();
        }

    }
}
