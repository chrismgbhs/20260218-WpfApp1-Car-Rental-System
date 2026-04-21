using _20260218_WpfApp1.Model;
using _20260218_WpfApp1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class RentCar : ObservableObject
    {
        public string RentalYear { get; set; }
        public string RentalMonth { get; set; }

        public string RentalDay { get; set; }

        public RentCar()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            SendCarToRentCommand = new RelayCommand(SendSelectedCarToRent);
        }

        public ICommand BackCommand { get; set; }
        public ICommand SendCarToRentCommand { get; set; }
        private Car _selectedCar;
        public Car car;

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
            Application.Current.Windows.OfType<View.RentCar>().FirstOrDefault()?.Close();                 // ✅ Close login after
        }

        public Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));

                if (SelectedCar != null)
                {
                    car = new Car(SelectedCar.Name, SelectedCar.Brand, SelectedCar.Age, SelectedCar.LicensePlate);
                    //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
                }
            }
        }

        public void SendSelectedCarToRent()
        {
            bool thirtyOneDays = false;
            int rentalYear;
            int rentalMonth;
            int rentalDay;

            //MessageBox.Show($"Selected Car:\nName: {car.Name}\nBrand: {car.Brand}\nAge: {car.Age}\nLicense Plate: {car.LicensePlate}");
            if (car != null)
            {
                if (RentalYear == null || RentalMonth == null || RentalDay == null)
                {
                    MessageBox.Show("Please fill in all the fields before sending the car to rent.");
                }

                else
                {

                    int.TryParse(RentalYear, out rentalYear);

                    if (rentalYear >= DateTime.Now.Year && rentalYear < DateTime.Now.Year + 2)
                    {
                        int.TryParse(RentalMonth, out rentalMonth);
                        bool goThrough = false;

                        if (rentalYear == DateTime.Now.Year)
                        {
                            if (rentalMonth >= DateTime.Now.Month && rentalMonth <= 12)
                            {
                                thirtyOneDays = Check31Days(rentalMonth);
                                goThrough = true;
                            }

                            else
                            {
                                MessageBox.Show("Invalid rental month.");
                            }
                        }

                        else
                        {
                            if (rentalMonth >= 1 && rentalMonth <= 12)
                            {
                                thirtyOneDays = Check31Days(rentalMonth);
                                goThrough = true;
                            }

                            else
                            {
                                MessageBox.Show("Invalid rental month input.");
                            }
                        }

                        if (goThrough)
                        {
                            bool continueProcess = false;
                            int monthDays;

                            if (thirtyOneDays)
                            {
                                monthDays = 31;
                            }

                            else
                            {
                                monthDays = 30;
                            }

                            int.TryParse(RentalDay, out rentalDay);

                            if (rentalYear == DateTime.Now.Year)
                            {

                                if (rentalMonth == 2 && (DateTime.Now.Year - 2024) % 4 != 0)
                                {
                                    monthDays = 28;
                                }

                                else
                                {
                                    monthDays = 29;
                                }


                                if (rentalMonth == DateTime.Now.Month)
                                {
                                    if (rentalDay >= DateTime.Now.Day && rentalDay <= monthDays)
                                    {
                                        continueProcess = true;
                                    }

                                    else
                                    {
                                        MessageBox.Show("Invalid rental day.");
                                    }
                                }

                                else
                                {
                                    if (rentalDay >= 1 && rentalDay <= monthDays)
                                    {
                                        continueProcess = true;
                                    }

                                    else
                                    {
                                        MessageBox.Show("Invalid rental day.");
                                    }
                                }
                            }

                            else
                            {
                                if (rentalMonth == 2 && (rentalYear - 2024) % 4 != 0)
                                {
                                    monthDays = 28;
                                }

                                else
                                {
                                    monthDays = 29;
                                }


                                if (rentalDay >= 1 && rentalDay <= monthDays)
                                {
                                    continueProcess = true;
                                }

                                else
                                {
                                    MessageBox.Show("Invalid rental day.");
                                }
                            }

                            if (continueProcess)
                            {
                                string startDateTime = $"{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}";
                                string endDateTime = $"{rentalMonth}/{rentalDay}/{rentalYear}";
                                Borrowed_Car borrowed_car = new Borrowed_Car(SelectedCar, LoginViewModel.CurrentUser.Username, startDateTime, endDateTime);
                                Cars_Out.carsRented.Add(borrowed_car);

                                MessageBox.Show($"{borrowed_car.Car.Name} has been rented on {borrowed_car.StartDateTime} until {borrowed_car.EndDateTime}.");

                                List<string> content = new List<string>();
                                content.Add(DateTime.Now.ToString());

                                content.Add($"Model: {borrowed_car.Car.Name} | Plate Number: {borrowed_car.Car.LicensePlate}");
                                content.Add($"Borrowed by {borrowed_car.BorrowerName} from {borrowed_car.StartDateTime} until {borrowed_car.EndDateTime}");

                                Cars_In.carsAvailable.Remove(SelectedCar);

                                File_Manager file_manager = new File_Manager("receipt.csv");
                                file_manager.Write(content, false);
                                MessageBox.Show("Receipt has been printed.");
                            }
                        }
                    }

                    else
                    {
                        MessageBox.Show("Invalid year. Please enter a valid year within two years.");
                    }
                }
            }
        }

        public static bool Check31Days(int rentalMonth)
        {
            bool thirtyOneDays = false;

            switch (rentalMonth)
            {
                case 1:
                    thirtyOneDays = true;
                    break;
                case 2:
                    thirtyOneDays = false;
                    break;
                case 3:
                    thirtyOneDays = true;
                    break;
                case 4:
                    thirtyOneDays = false;
                    break;
                case 5:
                    thirtyOneDays = true;
                    break;
                case 6:
                    thirtyOneDays = false;
                    break;
                case 7:
                    thirtyOneDays = true;
                    break;
                case 8:
                    thirtyOneDays = true;
                    break;
                case 9:
                    thirtyOneDays = false;
                    break;
                case 10:
                    thirtyOneDays = true;
                    break;
                case 11:
                    thirtyOneDays = false;
                    break;
                case 12:
                    thirtyOneDays = true;
                    break;
            }

            return thirtyOneDays;
        }
    }
}
