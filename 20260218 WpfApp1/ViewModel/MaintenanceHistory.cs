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
    internal class MaintenanceHistory : ObservableObject
    {
        public string _history;
        public string PlateNumber { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public string History
        {
            get { return _history; }
            set { _history = value; OnPropertyChanged(nameof(History)); }
        }

        public MaintenanceHistory()
        {
            SearchCommand = new RelayCommand(ExecuteSearch);
            BackCommand = new RelayCommand(ExecuteBack);
        }

        public void ExecuteSearch()
        {
            File_Manager file_Manager = new File_Manager($"File/{PlateNumber}.csv");
            List<string> lines = file_Manager.getLines();
            foreach (string line in lines)
            {
                History += $"{line}\n";
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
            Application.Current.Windows.OfType<View.MaintenanceHistory>().FirstOrDefault()?.Close();                 // ✅ Close login after
        }
    }
}
