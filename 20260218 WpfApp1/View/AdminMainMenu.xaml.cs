using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _20260218_WpfApp1.View
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class AdminMainMenu : Window
    {
        public AdminMainMenu()
        {
            InitializeComponent();
        }

        // SAVE AND EXIT
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // AVAILABLE CARS
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AvailableCars availableCars = new AvailableCars();
            availableCars.Show();
        }

        // RENTED CARS
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RentedCars rentedCars = new RentedCars();
            rentedCars.Show();
        }

        // CARS IN MAINTENANCE
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CarToMaintenance carToMaintenance = new CarToMaintenance();
            carToMaintenance.Show();
        }

        // MAINTENANCE HISTORY
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MaintenanceHistory maintenanceHistory = new MaintenanceHistory();
            maintenanceHistory.Show();
        }

        // CARS IN MAINTENANCE
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            CarsInMaintenance carsInMaintenance = new CarsInMaintenance();
            carsInMaintenance.Show();
        }

        // ADD A CAR
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            AddCar addCar = new AddCar();
            addCar.Show();
        }

        // ADD CARS
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            AddCars addCars = new AddCars();
            addCars.Show();
        }
    }
}
