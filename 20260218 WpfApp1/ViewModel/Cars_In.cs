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

        public static async Task InsertCarIntoDatabase(Car car)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"INSERT INTO cars_in (carID, modelName, brand, age, plateNumber) VALUES ((SELECT COUNT(*) + 1 FROM cars_in), @modelName, @brand, @age, @plateNumber)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.Parameters.AddWithValue("@modelName", car.Name);
                        command.Parameters.AddWithValue("@brand", car.Brand);
                        command.Parameters.AddWithValue("@age", car.Age);
                        command.Parameters.AddWithValue("@plateNumber", car.LicensePlate);
                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                        connection.Close();
                    }

                    MessageBox.Show($"Car with plate number: {car.LicensePlate} added to database.", car.LicensePlate, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }

        public static async Task DeleteCarFromDatabase(string plateNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"DELETE FROM cars_in WHERE plateNumber = @plateNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.Parameters.AddWithValue("@plateNumber", plateNumber);
                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                        connection.Close();
                    }
                }

                MessageBox.Show($"Car with plate number: {plateNumber} deleted from database.", plateNumber, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }

        public static async Task UpdateCarToDatabase (Car car)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"UPDATE cars_in SET modelname = @modelName, brand = @brand, age = @age, plateNumber = @plateNumber WHERE plateNumber = @plateNumber ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.Parameters.AddWithValue("@modelName", car.Name);
                        command.Parameters.AddWithValue("@brand", car.Brand);
                        command.Parameters.AddWithValue("@age", car.Age);
                        command.Parameters.AddWithValue("@plateNumber", car.LicensePlate);
                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                        connection.Close();
                    }
                }

                MessageBox.Show($"Car with plate number: {car.LicensePlate} updated in database.", car.LicensePlate, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }
        }

        public static async Task ExportCarsInList()
        {
            int internalCars_InCounter = Cars_In.carsAvailable.Count;
            int carsDiff = 0;
            int rows = 0;
            string query;
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    query = $"SELECT * FROM cars_in";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    bool carFound = false;

                                    foreach (Car car in Cars_In.carsAvailable)
                                    {
                                        if (car.LicensePlate == reader.GetString(reader.GetOrdinal("plateNumber")))
                                        {
                                            UpdateCarToDatabase(car);
                                        }

                                        else
                                        {
                                            InsertCarIntoDatabase(car);
                                        }
                                    }

                                    rows++;
                                }
                            }

                            else
                            {
                                foreach (Car car in Cars_In.carsAvailable)
                                {
                                    InsertCarIntoDatabase(car);
                                }
                            }
                        }

                        connection.Close();
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bool carFound = false;

                                foreach (Car car in Cars_In.carsAvailable)
                                {
                                    if (car.LicensePlate == reader.GetString(reader.GetOrdinal("plateNumber")))
                                    {
                                        carFound = true;
                                        break;
                                    }
                                }

                                if (carFound == false)
                                {
                                    DeleteCarFromDatabase(reader.GetString(reader.GetOrdinal("plateNumber")));
                                }
                            }
                        }

                        connection.Close();
                    }
                }  
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in: {ex.Message}");
                return;
            }

            //File_Manager file_Manager = new File_Manager("File/cars_in.csv");
            //List<string> lines = new List<string>();
            //foreach (Car car in carsAvailable)
            //{
            //    string line = $"{car.Name},{car.Brand},{car.Age},{car.LicensePlate}";
            //    lines.Add(line);
            //}
            //file_Manager.Write(lines, false);
        }

        public static void InitializeCarsInList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL.connectionString))
                {
                    string query = $"SELECT * FROM cars_in";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            MessageBox.Show($"{reader.FieldCount} cars found in database.");
                            while (reader.Read())
                            {
                                string modelName = reader.GetString(reader.GetOrdinal("modelName"));
                                string brand = reader.GetString(reader.GetOrdinal("brand"));
                                string age = reader.GetString(reader.GetOrdinal("age"));
                                string plateNumber = reader.GetString(reader.GetOrdinal("plateNumber"));

                                Car car = new Car (modelName, brand, age, plateNumber);

                                if (!Rental_Manager.CheckDuplicates(plateNumber))
                                {
                                    AddCar(car);
                                    MessageBox.Show($"Added car with plate number: {plateNumber} to inventory.", plateNumber, MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                                else
                                {
                                    MessageBox.Show($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
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

            //List<string> carLines = new List<string>();
            ////Console.WriteLine("Adding cars to inventory from file...");
            //File_Manager carFileManager = new File_Manager("File/cars_in.csv");

            //if (!carFileManager.Read())
            //{
            //    //Console.WriteLine("Error reading car data. Exiting...");
            //}

            //else
            //{
            //    carLines = carFileManager.getLines();

            //    foreach (string line in carLines)
            //    {
            //        //Thread.Sleep(100);
            //        string[] parts = line.Split(',');
            //        if (parts.Length >= 4)
            //        {
            //            string modelName = parts[0].Trim();
            //            string brand = parts[1].Trim();
            //            string age = parts[2].Trim();
            //            string plateNumber = parts[3].Trim();
            //            Car car = new Car(modelName, brand, age, plateNumber);

            //            if (!Rental_Manager.CheckDuplicates(plateNumber))
            //            {
            //                AddCar(car);
            //            }

            //            else
            //            {
            //                //Console.WriteLine($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
            //            }
            //        }

            //        else
            //        {
            //            //Console.WriteLine($"Invalid car data line: {line}");
            //        }
            //    }
            //}
        }
    }
}
