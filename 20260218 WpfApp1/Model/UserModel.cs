using _20260218_WpfApp1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260218_WpfApp1.Model
{
    internal class UserModel : ObservableObject
    {
        private string _username = string.Empty;
        private string _pin = string.Empty;

        public string _statusMessage = string.Empty;

        public string Username
        {

           get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Pin
        {
            get => _pin;
            set
            {
                if (_pin != value)
                {
                    _pin = value;
                    OnPropertyChanged(nameof(Pin));
                }
            }
        }
    }
}
