using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.ViewModel
{
    internal class Cars_in_Maintenance
    {
        public static ObservableCollection<Maintenance> carsInMaintenance = new ObservableCollection<Maintenance>();
        
        public static void AddMaintenance(Maintenance maintenance)
        {
            carsInMaintenance.Add(maintenance);
        }

        public static void InitializeMaintenancesList()
        {
            File_Manager file_Manager = new File_Manager("File/maintenances.csv");
            carsInMaintenance.Clear();

            foreach (string line in file_Manager.getLines())
            {
                Car car = new Car(line.Split(',')[0].Trim(), line.Split(',')[1].Trim(), line.Split(',')[2].Trim(), line.Split(',')[3].Trim());
                Maintenance maintenance = new Maintenance(car, line.Split(',')[4].Trim(), line.Split(',')[5].Trim(), line.Split(',')[6].Trim());
                AddMaintenance(maintenance);
            }
        }

        public static void ExportMaintenancesList()
        {
            File_Manager file_Manager = new File_Manager("File/maintenances.csv");

            List<string> lines = new List<string>();

            foreach (Maintenance maintenance in carsInMaintenance)
            {
                lines.Add($"{maintenance.Car.Name},{maintenance.Car.Brand},{maintenance.Car.Age},{maintenance.Car.LicensePlate},{maintenance.MaintenanceDetails},{maintenance.MaintenanceWorker},{maintenance.StartDate}");
            }

            file_Manager.Write(lines, false);
        }

    }
}
