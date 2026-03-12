using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class NewWindow
    {
        public LostItem LostItem{ get; set; }
        public ICommand SaveCommand { get; set; }

        public ObservableCollection<LostItem> LostItems { get; set; }

        public NewWindow()
        {
            LostItem = new LostItem();
            SaveCommand = new RelayCommand(SaveLostItem);

            LostItems = new ObservableCollection<LostItem>()
            { 
                new LostItem
                {   
                    Name = "Wallet",
                    Description = "Black leather wallet with multiple card slots and a zippered coin pocket.",
                    Location = "Central Park",
                    Time = "2024-06-15 14:30"
                },

                new LostItem
                {
                    Name = "iPhone",
                    Description = "Apple phone",
                    Location = "Library",
                    Time = "2024-06-16 10:00"
                }
            };
        }

        private void SaveLostItem()
        {
            LostItems.Add(new LostItem
            {
                Name = LostItem.Name,
                Description = LostItem.Description,
                Location = LostItem.Location,
                Time = LostItem.Time
            });

            // Clear the input fields after saving
            LostItem.Name = string.Empty;
            LostItem.Description = string.Empty;
            LostItem.Location = string.Empty;
            LostItem.Time = string.Empty;
        }
    }
}
