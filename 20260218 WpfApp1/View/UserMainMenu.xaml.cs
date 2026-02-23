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
    public partial class UserMainMenu : Window
    {
        public UserMainMenu()
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RentedCars rentedCars = new RentedCars();
            rentedCars.Show();
        }

        // RENT A CAR
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RentCar rentCar = new RentCar();
            rentCar.Show();
        }

        // RETURN A CAR
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ReturnCar returnCar = new ReturnCar();
            returnCar.Show();
        }
    }
}
