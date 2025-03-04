using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using K4os.Compression.LZ4.Internal;
using Mysqlx;
using Mysqlx.Crud;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VetManagement.ViewModels
{
    public class CreateMedViewModel : ViewModelBase
    {

        private List<string> _customErrors = new();

        private bool _isVisibleForm = false;

        private readonly Action<Med> _onMedCreated;
        public ObservableCollection<MedWrapper> Meds { get; set; } = new ObservableCollection<MedWrapper>();

        private int _invoiceNumber;
        public int InvoiceNumber
        {
            get => _invoiceNumber;
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged(nameof(InvoiceNumber));
            }
        }

        private string _provider;

        public string Provider
        {
            get => _provider;
            set
            {
                _provider = value;
                OnPropertyChanged(nameof(Provider));
            }
        }

        private long _invoiceDate = (long)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

        public long InvoiceDate
        {
            get => _invoiceDate;
            set
            {
                _invoiceDate = value;
                OnPropertyChanged(nameof(InvoiceDate));
            }
        }

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

        public bool isVisibleForm
        {
            get => _isVisibleForm;
            set
            {
                _isVisibleForm = value;
                OnPropertyChanged(nameof(isVisibleForm));
            }
        }

        public ICommand CreateMedCommand { get; set; }
        public ICommand ToggleFormVisibilityCommand { get; }
        public ICommand AddMedsCommand { get; }
        public ICommand InsertNewMedCommand { get; }

        public ObservableCollection<object> PieceTypeList { get; set; } =
            new ObservableCollection<object>  { new { Name = "Flacoane", Value = "flacoane" }, new { Name = "Comprimate", Value = "comprimate" } } ;
        public ObservableCollection<object> MedTypeList { get; set; } =
            new ObservableCollection<object>  { new { Name = "Medicament", Value = "medicament" }, new { Name = "Vaccin", Value = "vaccin" } } ;  

        public CreateMedViewModel(Action<Med> onMedCreated)
        {
            _onMedCreated = onMedCreated;
            CreateMedCommand = new RelayCommand(CreateMeds);
            AddMedsCommand = new RelayCommand(AddMeds);
            InsertNewMedCommand = new RelayCommand(InsertNewMed);

            Meds.Add(new MedWrapper(new Med { Type = "medicament", PieceType = "flacoane", Valability = (long)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()} ));

            Meds.CollectionChanged += AddEmptyMedToList;

        }

        private void InsertNewMed(object paramete)
        {
            Meds.Add(new MedWrapper(new Med { Type = "medicament", PieceType = "flacoane", Valability = (long)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() }));
        }

        private void AddEmptyMedToList(object parameter, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var lastMed = Meds.Last();

                lastMed = new MedWrapper(new Med { Type = "medicament", PieceType = "flacoane", Valability = (long)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() });
            }
        }

        private void AddMeds(object parameter)
        {

            foreach (MedWrapper med in Meds ) 
            {
                Trace.WriteLine(med.Name);
                Trace.WriteLine(med.Valability);
                Trace.WriteLine(med.PieceType);
            }

        }
  
        private bool CustomValidation(Med med)
        {
            if (med.Pieces <= 0)
            {
                _customErrors.Add( "Cantiatea in bucăți trebuie sa fie mai mare decat 0!");
            }

            if (med.PerPiece <= 0)
            {
                _customErrors.Add("Cantiatea per bucată trebuie sa fie mai mare decat 0!");
            }

            return _customErrors.Count() == 0;
        }

        private bool Validate(Med med)
        {
            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(med, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(med, context, validationResults);

            bool customValidation = CustomValidation(med);
            if (!isValid || !customValidation)
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


        private async void CreateMeds(object parameter)
        {

            int index = 1;
            foreach( MedWrapper medWrapper in Meds)
            {
                if (!Validate(medWrapper.Med))
                {
                    string message = "Completați câmpurile necesare pentru medicamentul " + index + "!\n";

                    message += string.Join("\n", Errors.Select(kv => kv.Value.FirstOrDefault())) + "\n"; 
                    message += string.Join("\n", _customErrors); 

                    Boxes.InfoBox(message);
                    Errors.Clear();
                    _customErrors.Clear();
                    return;
                }
                index++;
            }

            try 
            {
                BaseRepository<Med> medRepository = new BaseRepository<Med>();
                BaseRepository<Invoice> invoiceRepository = new BaseRepository<Invoice>();

                Invoice Invoice = await invoiceRepository.Add(new Invoice() { Number = InvoiceNumber, Date = InvoiceDate, Provider = Provider, ProductCount = Meds.Count() });

                if( Invoice == null)
                {
                    Boxes.ErrorBox("Factura nu a putut fi creeată!");
                    return;
                }

                foreach (MedWrapper medWrapper in Meds)
                {
                    medWrapper.Med.DateAdded = (long)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                    medWrapper.Med.InvoiceNumber = InvoiceNumber; 
                    await medRepository.Add(medWrapper.Med);
                    _onMedCreated?.Invoke(medWrapper.Med);
                }

                Boxes.InfoBox("Medicamentele au fost adăugate!");

            }

            catch (Exception ex)
            {
                Logger.LogError("Error", ex.ToString());
                Boxes.ErrorBox("Medicamentul nu a putut fi adăugat!\n" + ex.Message); 

            }
        }
    }
}
