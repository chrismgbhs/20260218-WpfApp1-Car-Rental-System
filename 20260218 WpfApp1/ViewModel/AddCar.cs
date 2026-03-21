using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace _20260218_WpfApp1.ViewModel
{
    internal class AddCar
    {
        public AddCar()
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
            Application.Current.Windows.OfType<View.AddCar>().FirstOrDefault()?.Close();                 // ✅ Close login after
        }
    }
}
