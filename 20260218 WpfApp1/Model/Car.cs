using _20260218_WpfApp1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.Model
{
    internal class Car : ObservableObject
    {
        private string _name;
        private string _brand;
        private string _age;
        private string _licensePlate;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Brand
        {
            get => _brand;
            set { _brand = value; OnPropertyChanged(nameof(Brand)); }
        }

        public string Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }

        public string LicensePlate
        {
            get => _licensePlate;
            set { _licensePlate = value; OnPropertyChanged(nameof(LicensePlate)); }
        }

        public Car(string name, string brand, string age, string licensePlate)
        {
            _name = name;
            _brand = brand;
            _age = age;
            _licensePlate = licensePlate;
        }
    }
}