using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class LoginViewModel : ObservableObject
    {
        public UserModel CurrentUser { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            CurrentUser = new UserModel();  
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin()
        {
            bool userFound = false;
            File_Manager file_Manager = new File_Manager("File/users.csv");
            List<string> lines = file_Manager.getLines();
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                string[] userDetails = line.Trim().Split(',');
                if (userDetails[0] == CurrentUser.Username && userDetails[1] == CurrentUser.Pin)
                {
                    userFound = true;
                    MessageBox.Show("User found.");

                    if (userDetails[2] == "Admin")
                    {
                        var AdminMainMenu = new AdminMainMenu();
                        AdminMainMenu.Show();
                    }

                    else
                    {
                        var UserMainMenu = new UserMainMenu();
                        UserMainMenu.Show();
                    }
                    break;
                }
            }

            if (!userFound)
            {
                MessageBox.Show("User not found. Please check your username and PIN.");
            }

        }
    }
}
