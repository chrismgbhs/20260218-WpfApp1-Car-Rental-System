using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class LoginViewModel : ObservableObject
    {
        //Declare and construct the objects using a new name.
        public static UserModel CurrentUser { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            CurrentUser = new UserModel();  
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        public async Task GoLogin()
        {
            bool userFound = false;

            DatabaseManager.Login(CurrentUser, out userFound);

            if (!userFound)
            {
                MessageBox.Show("User not found. Please check your username and PIN.");
            }
        }

        //Implement method and logic.
        private async void ExecuteLogin()
        {
            await GoLogin();
        }
    }
}
