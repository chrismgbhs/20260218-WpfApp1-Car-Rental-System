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
        string connectionString = @"Server=CCL2-10\MSSQLSERVER01; Database=Car Rental System; User Id=sa; Password=ccl2; TrustServerCertificate=True;";

        public LoginViewModel()
        {
            CurrentUser = new UserModel();  
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        //Implement method and logic.
        private void ExecuteLogin()
        {
            bool userFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"SELECT * FROM Users WHERE Username = @username AND Pin = @pin";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", CurrentUser.Username);
                        command.Parameters.AddWithValue("@pin", CurrentUser.Pin);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show("User found.");
                                Cars_In.InitializeCarsInList();
                                Cars_Out.InitializeCarsOutList();
                                Cars_in_Maintenance.InitializeMaintenancesList();

                                userFound = true;

                                while (reader.Read())
                                {
                                    if (reader.GetString(reader.GetOrdinal("Role")) == "admin")
                                    {
                                        CurrentUser.Role = "admin";
                                        var AdminMainMenu = new AdminMainMenu();
                                        Application.Current.MainWindow = AdminMainMenu; // ✅ Set BEFORE closing
                                        AdminMainMenu.Show();                           // ✅ Non-blocking
                                        Application.Current.Windows
                                            .OfType<Login>()
                                            .FirstOrDefault()?.Close();                 // ✅ Close login after
                                    }

                                    else
                                    {
                                        var mainWindow = new UserMainMenu();
                                        Application.Current.MainWindow = mainWindow; // ✅ Set BEFORE closing
                                        mainWindow.Show();                           // ✅ Non-blocking
                                        Application.Current.Windows
                                            .OfType<Login>()
                                            .FirstOrDefault()?.Close();                 // ✅ Close login after
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }

            if (!userFound)
            {
                MessageBox.Show("User not found. Please check your username and PIN.");
            }
        }
    }
}
