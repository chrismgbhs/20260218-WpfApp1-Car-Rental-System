using _20260218_WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace _20260218_WpfApp1.ViewModel
{
    internal class NewWindow : ObservableObject
    {
        public LostItem LostItem{ get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<LostItem> LostItems { get; set; }

        private LostItem _selectedLostItems;
        public LostItem SelectedLostItems
        {
            get { return _selectedLostItems; }
            set
            {
                _selectedLostItems = value;
                OnPropertyChanged(nameof(SelectedLostItems));

                if (SelectedLostItems != null)
                {
                    LostItem.Name = SelectedLostItems.Name;
                    LostItem.Description = SelectedLostItems.Description;
                    LostItem.Location = SelectedLostItems.Location;
                    LostItem.Time = SelectedLostItems.Time;
                }
            }
        }

        public NewWindow()
        {
            LostItem = new LostItem();
            SaveCommand = new RelayCommand(SaveLostItem);
            ClearCommand = new RelayCommand(ClearFields);
            DeleteCommand = new RelayCommand(DeleteLostItem);

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
            if (LostItem.Name == "" || LostItem.Description == "" || LostItem.Location == "")
            {
                MessageBox.Show("Please fill in all fields before saving.");
            }

            else
            {
                LostItems.Add(new LostItem
                {
                    Name = LostItem.Name,
                    Description = LostItem.Description,
                    Location = LostItem.Location,
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                });

                // Clear the input fields after saving
                ClearFields();
            }
        }

        private void ClearFields()
        {
            LostItem.Name = string.Empty;
            LostItem.Description = string.Empty;
            LostItem.Location = string.Empty;
            LostItem.Time = string.Empty;
        }

        private void DeleteLostItem()
        {

           if (SelectedLostItems != null)
            {
                LostItems.Remove(SelectedLostItems);
                ClearFields();
            }

            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }
    }
}
