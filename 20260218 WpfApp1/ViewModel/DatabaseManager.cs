using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Windows;
using _20260218_WpfApp1.ViewModel;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using _20260218_WpfApp1.View;
using System.Security.Cryptography.X509Certificates;

namespace _20260218_WpfApp1.ViewModel
{
    internal class DatabaseManager
    {
        public static List<string> History = new List<string>();

        //REFRESH DATABASE
        public static async Task RefreshDatabase()
        {
            await ExportCarsToDatabase();
            await ExportCarsOut();
            await ExportMaintenances();
            MessageBox.Show("Database refreshed successfully.");
        }

        //LOGIN
        public static void Login(UserModel CurrentUser, out bool userFound)
        {
            userFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
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
                MessageBox.Show($"{ex.Message}");
                return;
            }
        }

        //CARS IN (AVAILABLE CARS)

        public static async Task InsertCarIntoDatabase(Car car)
        {
            string query;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"INSERT INTO cars_in (modelName, brand, age, plateNumber) VALUES (@modelName, @brand, @age, @plateNumber)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@modelName", car.Name);
                        command.Parameters.AddWithValue("@brand", car.Brand);
                        command.Parameters.AddWithValue("@age", car.Age);
                        command.Parameters.AddWithValue("@plateNumber", car.LicensePlate);
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task ExportCarsToDatabase()
        {
            string query;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"DROP TABLE IF EXISTS cars_in";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                    return;
                }

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"CREATE TABLE cars_in (modelName NVARCHAR(100), brand NVARCHAR(100), age NVARCHAR(50), plateNumber NVARCHAR(50))";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                
                foreach (Car car in Cars_In.carsAvailable)
                {
                    await InsertCarIntoDatabase(car);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task InitializeCarsIn()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"SELECT * FROM cars_in";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string modelName = reader.GetString(reader.GetOrdinal("modelName"));
                                string brand = reader.GetString(reader.GetOrdinal("brand"));
                                string age = reader.GetString(reader.GetOrdinal("age"));
                                string plateNumber = reader.GetString(reader.GetOrdinal("plateNumber"));

                                Car car = new Car(modelName, brand, age, plateNumber);

                                if (!Rental_Manager.CheckDuplicates(plateNumber))
                                {
                                    Cars_In.AddCar(car);
                                }

                                else
                                {
                                    MessageBox.Show($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
                                }
                            }
                        }

                        connection.Close();
                        MessageBox.Show("Cars in list initialized from database.");
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }

        //CARS OUT (RENTS)
        public static async Task InsertCarIntoDatabaseOut(Borrowed_Car car)
        {
            string query;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"INSERT INTO cars_out (modelName, brand, age, plateNumber, borrowerName, startDateTime, endDateTime) VALUES (@modelName, @brand, @age, @plateNumber, @borrowerName, @startDateTime, @endDateTime)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@modelName", car.Car.Name);
                        command.Parameters.AddWithValue("@brand", car.Car.Brand);
                        command.Parameters.AddWithValue("@age", car.Car.Age);
                        command.Parameters.AddWithValue("@plateNumber", car.Car.LicensePlate);
                        command.Parameters.AddWithValue("@borrowerName", car.BorrowerName);
                        command.Parameters.AddWithValue("@startDateTime", car.StartDateTime);
                        command.Parameters.AddWithValue("@endDateTime", car.EndDateTime);
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task InitializeCarsOut()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"SELECT * FROM cars_out";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string modelName = reader.GetString(reader.GetOrdinal("modelName"));
                                string brand = reader.GetString(reader.GetOrdinal("brand"));
                                string age = reader.GetString(reader.GetOrdinal("age"));
                                string plateNumber = reader.GetString(reader.GetOrdinal("plateNumber"));
                                string borrowerName = reader.GetString(reader.GetOrdinal("borrowerName"));
                                string startDateTime = reader.GetString(reader.GetOrdinal("startDateTime"));
                                string endDateTime = reader.GetString(reader.GetOrdinal("endDateTime"));

                                Car car = new Car(modelName, brand, age, plateNumber);
                                Borrowed_Car borrowed_Car = new Borrowed_Car(car, borrowerName, startDateTime, endDateTime);

                                if (!Rental_Manager.CheckDuplicates(plateNumber))
                                {
                                    Cars_Out.carsRented.Add(borrowed_Car);
                                }

                                else
                                {
                                    MessageBox.Show($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
                                }
                            }
                        }

                        connection.Close();
                        MessageBox.Show("Cars out list initialized from database.");
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task ExportCarsOut()
        {
            string query;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"DROP TABLE IF EXISTS cars_out";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"CREATE TABLE cars_out (modelName NVARCHAR(100), brand NVARCHAR(100), age NVARCHAR(50), plateNumber NVARCHAR(50), borrowerName NVARCHAR(50), startDateTime NVARCHAR(50), endDateTime NVARCHAR(50))";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                foreach (Borrowed_Car car in Cars_Out.carsRented)
                {
                    await InsertCarIntoDatabaseOut(car);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }

        //MAINTENANCE
        public static async Task MaintenanceHistory()
        {
            History.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string plateNumber = ViewModel.MaintenanceHistory.PlateNumber;
                    File_Manager file_Manager = new File_Manager($"{plateNumber}.csv");
                    string query = $"SELECT * FROM maintenances WHERE plateNumber = @plateNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@plateNumber", plateNumber);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string modelName = reader.GetString(reader.GetOrdinal("modelName"));
                                string brand = reader.GetString(reader.GetOrdinal("brand"));
                                string age = reader.GetString(reader.GetOrdinal("age"));
                                string licensePlate = reader.GetString(reader.GetOrdinal("plateNumber"));
                                string maintenanceDetails = reader.GetString(reader.GetOrdinal("maintenanceDetails"));
                                string maintenanceWorker = reader.GetString(reader.GetOrdinal("maintenanceWorker"));
                                string startDate = reader.GetString(reader.GetOrdinal("startDate"));
                                string endDate = reader.GetString(reader.GetOrdinal("endDate"));

                                History.Add($"Model: {modelName}, Brand: {brand}, Age: {age}, Plate Number: {licensePlate}, Maintenance Details: {maintenanceDetails}, Maintenance Worker: {maintenanceWorker}, Start Date: {startDate}, End Date: {endDate}\n");
                            }

                            file_Manager.Write(History, false);

                            MessageBox.Show($"Maintenance history for plate number {plateNumber} has been written to the file.");
                        }

                        connection.Close();
                        MessageBox.Show("Maintenance history initialized from database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task AddToHistory(Maintenance maintenance)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"INSERT INTO maintenances (modelName, brand, age, plateNumber, maintenanceDetails, maintenanceWorker, startDate, endDate) VALUES (@modelName, @brand, @age, @plateNumber, @maintenanceDetails, @maintenanceWorker, @startDate, @endDate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@modelName", maintenance.Car.Name);
                        command.Parameters.AddWithValue("@brand", maintenance.Car.Brand);
                        command.Parameters.AddWithValue("@age", maintenance.Car.Age);
                        command.Parameters.AddWithValue("@plateNumber", maintenance.Car.LicensePlate);
                        command.Parameters.AddWithValue("@maintenanceDetails", maintenance.MaintenanceDetails);
                        command.Parameters.AddWithValue("@maintenanceWorker", maintenance.MaintenanceWorker);
                        command.Parameters.AddWithValue("@startDate", maintenance.StartDate);
                        command.Parameters.AddWithValue("@endDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task InitializeMaintenances()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"SELECT * FROM maintenance";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string modelName = reader.GetString(reader.GetOrdinal("modelName"));
                                string brand = reader.GetString(reader.GetOrdinal("brand"));
                                string age = reader.GetString(reader.GetOrdinal("age"));
                                string plateNumber = reader.GetString(reader.GetOrdinal("plateNumber"));
                                string maintenanceDetails = reader.GetString(reader.GetOrdinal("maintenanceDetails"));
                                string maintenanceWorker = reader.GetString(reader.GetOrdinal("maintenanceWorker"));
                                string startDate = reader.GetString(reader.GetOrdinal("startDate"));

                                Car car = new Car(modelName, brand, age, plateNumber);
                                Maintenance maintenance = new Maintenance(car, maintenanceDetails, maintenanceWorker, startDate);

                                if (!Rental_Manager.CheckDuplicates(plateNumber))
                                {
                                    Cars_in_Maintenance.AddMaintenance(maintenance);
                                }

                                else
                                {
                                    MessageBox.Show($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
                                }
                            }
                        }

                        connection.Close();
                        MessageBox.Show("Maintenance list initialized from database.");
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task InsertCarIntoDatabaseMaintenance(Maintenance maintenance)
        {
            string query;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"INSERT INTO maintenance (modelName, brand, age, plateNumber, maintenanceDetails, maintenanceWorker, startDate) VALUES (@modelName, @brand, @age, @plateNumber, @maintenanceDetails, @maintenanceWorker, @startDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@modelName", maintenance.Car.Name);
                        command.Parameters.AddWithValue("@brand", maintenance.Car.Brand);
                        command.Parameters.AddWithValue("@age", maintenance.Car.Age);
                        command.Parameters.AddWithValue("@plateNumber", maintenance.Car.LicensePlate);
                        command.Parameters.AddWithValue("@maintenanceDetails", maintenance.MaintenanceDetails);
                        command.Parameters.AddWithValue("@maintenanceWorker", maintenance.MaintenanceWorker);
                        command.Parameters.AddWithValue("@startDate", maintenance.StartDate);
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
        public static async Task ExportMaintenances()
        {
            string query;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"DROP TABLE IF EXISTS maintenance ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"CREATE TABLE maintenance (modelName NVARCHAR(100), brand NVARCHAR(100), age NVARCHAR(50), plateNumber NVARCHAR(50), maintenanceDetails NVARCHAR(50), maintenanceWorker NVARCHAR(50), startDate NVARCHAR(50))";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                foreach (Maintenance maintenance in Cars_in_Maintenance.carsInMaintenance)
                {
                    await InsertCarIntoDatabaseMaintenance(maintenance);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }
    }
}
