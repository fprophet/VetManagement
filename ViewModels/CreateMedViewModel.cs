using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Mysqlx.Crud;
using VetManagement.Data;
using VetManagement.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VetManagement.ViewModels
{
    public class CreateMedViewModel : ViewModelBase
    {
        private bool _isVisibleForm = false;

        private string _name; 

        private string _description;

        private DateTime _valability = DateTime.Today;

        private string _lotID;

        private int _dateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        private int _dateUpdated = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        private readonly Action<Med> _onMedCreated;

        public ICommand CreateMedCommand { get; set; }
        public ICommand ToggleFormVisibilityCommand { get; }

        public ObservableCollection<object> PieceTypeList { get; set; } =
            new ObservableCollection<object>  { new { Name = "Flacoane", Value = "flacoane" }, new { Name = "Comprimate", Value = "comprimate" } } ;
        public ObservableCollection<object> MedTypeList { get; set; } =
            new ObservableCollection<object>  { new { Name = "Medicament", Value = "medicament" }, new { Name = "Vaccin", Value = "vaccin" } } ;  

        private bool _isVisiblePerPieceInput = true;
        public bool isVisiblePerPieceInput
        {
            get => _isVisiblePerPieceInput;
            set
            {
                _isVisiblePerPieceInput = value;
                OnPropertyChanged(nameof(isVisiblePerPieceInput));
            }
        }

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
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _pieceType = "flacoane";
        public string PieceType
        {
            get => _pieceType;
            set
            {
                _pieceType = value;
                OnPropertyChanged(nameof(PieceType));
                if( value == "comprimate")
                {
                    _perPiece = 1;
                    isVisiblePerPieceInput = false;
                    TotalAmountUnit = "comprimate";
                }
                else
                {
                    _perPiece = 0;
                    isVisiblePerPieceInput = true;
                    TotalAmountUnit = "ml";
                }

                CalculateTotalAmount();

            }
        }

        private int _pieces;
        public int Pieces
        {
            get => _pieces;
            set
            {
                _pieces = value;
                OnPropertyChanged(nameof(Pieces));
                CalculateTotalAmount();
            }
        }

        private float _perPiece;
        public float PerPiece
        {
            get => _perPiece;
            set
            {
                _perPiece = value;
                OnPropertyChanged(nameof(_perPiece));
                CalculateTotalAmount();
            }
        }

        private string _perPieceString;
        public string PerPieceString
        {
            get => _perPieceString;
            set
            {
                float parsed;
                if (float.TryParse(value, out parsed))
                {
                    _perPieceString = value;
                    PerPiece = parsed;
                    OnPropertyChanged(nameof(PerPieceString));
                }
            }
        }

        private float _totalAmount;
        public float TotalAmount
        {
            get => _totalAmount;
            set 
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private string _totalAmountUnit = "ml";
        public string TotalAmountUnit
        {
            get => _totalAmountUnit;
            set
            {
                _totalAmountUnit = value;
                OnPropertyChanged(nameof(TotalAmountUnit));
            }
        }

        private string _type = "medicament";
        public string Type 
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public int DateAdded
        {
            get => _dateAdded;
            set
            {
                _dateAdded = value;
                OnPropertyChanged(nameof(DateAdded));
            }
        }

        public int DateUpdated
        {
            get => _dateUpdated;
            set
            {
                _dateUpdated = value;
                OnPropertyChanged(nameof(DateUpdated));
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

        public DateTime Valability
        {
            get => _valability;
            set
            {
                _valability = value;
                OnPropertyChanged(nameof(Valability));
            }
        }

        public bool isVisibleForm
        {
            get => _isVisibleForm;
            set
            {
                _isVisibleForm = value;
                OnPropertyChanged(nameof(isVisibleForm));
            }
        }
        public CreateMedViewModel(Action<Med> onMedCreated)
        {
            _onMedCreated = onMedCreated;
            CreateMedCommand = new RelayCommand(CreateMed, CanExecuteCreateMed);
            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);

        }

        private void CalculateTotalAmount()
        {
            TotalAmount = Pieces * PerPiece;
        }

        private void ToggleFormVisibility(object parameter)
        {
            isVisibleForm = !isVisibleForm;
        }

        private bool CanExecuteCreateMed(object parameter)
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(PieceType) && Pieces > 0;
        }

        private bool Validate(Med med)
        {
            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(med, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(med, context, validationResults);

            if (!isValid)
            {
                foreach (var error in validationResults)
                {
                    Errors.Add(error.MemberNames.First(), new List<string> { error.ErrorMessage });
                    OnErrorsChanged(error.MemberNames.First());
                }
                return false;
            }

            return true;
        }


        private async void CreateMed(object parameter)
        {
            long x = (long)((DateTimeOffset)Valability).ToUnixTimeSeconds();
            Med med = new Med() 
             { 
                Name = Name,
                Type = Type,
                PieceType = PieceType, 
                Pieces = Pieces,
                PerPiece = PerPiece, 
                TotalAmount = TotalAmount, 
                DateAdded = DateAdded,
                Description = Description, 
                LotID =LotID, 
                Valability = (long)((DateTimeOffset)Valability).ToUnixTimeSeconds()
            };

            if ( !Validate(med))
            {
                return;
            }

            try 
            {
                await new BaseRepository<Med>().Add(med);
                _onMedCreated?.Invoke(med);

                Boxes.InfoBox("Medicamentul a fost adăugat!");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex.ToString());
                MessageBox.Show("Medicamentul nu a putut fi adăugat!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
