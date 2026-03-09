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
        //DECLARATION OF OBJECT ELEMENTS
        private string _username = string.Empty;
        private string _pin = string.Empty;

        //REFERENCE USERNAME TO PROPERTY CHANGE
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

        //REFERENCE PIN TO PROPERTY CHANGE
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
