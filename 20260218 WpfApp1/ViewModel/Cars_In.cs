using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Cars_In : ObservableObject
    {
        public static ObservableCollection<Car> carsAvailable = new ObservableCollection<Car>();

        public static void AddCar(Car car)
        {
            carsAvailable.Add(car);
        }

        public static void ExportCarsInList()
        {
            File_Manager file_Manager = new File_Manager("File/cars_in.csv");
            List<string> lines = new List<string>();
            foreach (Car car in carsAvailable)
            {
                string line = $"{car.Name},{car.Brand},{car.Age},{car.LicensePlate}";
                lines.Add(line);
            }
            file_Manager.Write(lines, false);
        }

        public static void InitializeCarsInList()
        {
            List<string> carLines = new List<string>();
            //Console.WriteLine("Adding cars to inventory from file...");
            File_Manager carFileManager = new File_Manager("File/cars_in.csv");

            if (!carFileManager.Read())
            {
                //Console.WriteLine("Error reading car data. Exiting...");
            }

            else
            {
                carLines = carFileManager.getLines();

                foreach (string line in carLines)
                {
                    //Thread.Sleep(100);
                    string[] parts = line.Split(',');
                    if (parts.Length >= 4)
                    {
                        string modelName = parts[0].Trim();
                        string brand = parts[1].Trim();
                        string age = parts[2].Trim();
                        string plateNumber = parts[3].Trim();
                        Car car = new Car(modelName, brand, age, plateNumber);

                        if (!Rental_Manager.CheckDuplicates(plateNumber))
                        {
                            AddCar(car);
                        }

                        else
                        {
                            //Console.WriteLine($"Duplicate car found with plate number: {plateNumber}. Skipping addition.");
                        }
                    }

                    else
                    {
                        //Console.WriteLine($"Invalid car data line: {line}");
                    }
                }
            }
        }
    }
}
