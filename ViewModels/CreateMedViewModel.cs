using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VetManagement.Data;
using VetManagement.Services;

namespace VetManagement.ViewModels
{
    public class CreateMedViewModel : ViewModelBase
    {

        private string _name; 

        private string _description;

        private string _quantityType;

        private float _quantity;

        private string _valability;

        private string _lotID;

        private int _dateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        private int _dateUpdated = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        private readonly BaseRepository<Med> _medRepository = new BaseRepository<Med>();

        private readonly Action<Med> _onMedCreated;

        public ICommand CreateMedCommand { get; set; }

        public string Name 
        { 
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));  
            }
        }
        
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string QuantityType
        {
            get => _quantityType;
            set
            {
                _quantityType = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public float Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int DateAdded
        {
            get => _dateAdded;
            set
            {
                _dateAdded = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int DateUpdated
        {
            get => _dateUpdated;
            set
            {
                _dateUpdated = value;
                OnPropertyChanged(nameof(Name));
            }
        } 
        
        public string LotID
        {
            get => _lotID;
            set
            {
                _lotID = value;
                OnPropertyChanged(nameof(LotID));
            }
        }

        public string Valability
        {
            get => _valability;
            set
            {
                _valability = value;
                OnPropertyChanged(nameof(Valability));
            }
        }

        public CreateMedViewModel(Action<Med> onMedCreated)
        {
            _onMedCreated = onMedCreated;
            CreateMedCommand = new RelayCommand(CreateMed, CanExecuteCreateMed);
            
        }

        private bool CanExecuteCreateMed(object parameter)
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(QuantityType) && Quantity > 0;
        }

        private async void CreateMed(object parameter)
        { 
            Med med = new Med() { Name = Name, QuantityType = QuantityType, Quantity = Quantity, DateAdded = DateAdded, Description = Description, LotID =LotID, Valability = Valability };
            Trace.WriteLine(med);
            try 
            {
                await _medRepository.Add(med);
                _onMedCreated?.Invoke(med);

            }catch (Exception ex)
            {
                Logger.LogError("Error", ex.ToString());
                MessageBox.Show("Medicamentul nu a putut fi adăugat!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
