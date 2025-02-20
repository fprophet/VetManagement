using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Google.Protobuf.Compiler;
using Mysqlx.Connection;
using Mysqlx.Crud;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class CreateRegistryRecordViewModel : ViewModelBase
    {
        private ObservableCollection<Owner> _owners = new ObservableCollection<Owner>();
        public ObservableCollection<Owner> Owners
        { 
            get => _owners; 
            set
            {
                _owners = value;
                OnPropertyChanged(nameof(Owners));
            }
        } 

        public ObservableCollection<Med> Meds { get; set; } = new ObservableCollection<Med>();

        public ObservableCollection<MedInputPair> MedInputPair { get; set; } = new ObservableCollection<MedInputPair>();

        private Action<RegistryRecord> OnCreateRegistryRecord;

        private Owner _selectedOwner;
        public Owner SelectedOwner
        { 
            get => _selectedOwner;
            set
            {
                _selectedOwner = value;
                OnPropertyChanged(nameof(SelectedOwner));
            }
        }

        private string _species;
        public string Species
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged(nameof(Species));
            }
        }

        private string _sex;
        public string Sex
        {
            get => _sex;
            set
            {
                _sex = value;
                OnPropertyChanged(nameof(Sex));
            }
        }

        private float _age;
        public float Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private string _identifier;
        public string Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                OnPropertyChanged(nameof(Identifier));
            }
        }

        private int _recipeNumber;
        public int RecipeNumber
        {
            get => _recipeNumber;
            set
            {
                _recipeNumber = value;
                OnPropertyChanged(nameof(RecipeNumber));
            }
        }

        private DateTime _recipeDate = DateTime.Today;
        public DateTime RecipeDate
        {
            get => _recipeDate;
            set
            {
                _recipeDate = value;
                OnPropertyChanged(nameof(RecipeDate));
            }
        }

        private string _treatmentDuration;
        public string TreatmentDuration
        {
            get => _treatmentDuration;
            set
            {
                _treatmentDuration = value;
                OnPropertyChanged(nameof(TreatmentDuration));
            }
        }

        private string _outcome;
        public string Outcome
        {
            get => _outcome;
            set
            {
                _outcome = value;
                OnPropertyChanged(nameof(Outcome));
            }
        }

        private string _medName;
        public string MedName
        {
            get => _medName;
            set
            {
                _medName = value;
                OnPropertyChanged(nameof(MedName));
            }
        }

        private string _symptoms;
        public string Symptoms
        {
            get => _symptoms;
            set
            {
                _symptoms = value;
                OnPropertyChanged(nameof(Symptoms));
            }
        }

        private string _observations;
        public string Observations
        {
            get => _observations;
            set
            {
                _observations = value;
                OnPropertyChanged(nameof(Observations));
            }
        }

        public ICommand AddInputPairCommand { get; }

        public ICommand RemoveInputPairCommand { get; }

        public ICommand CreateRegistryRecordCommand { get; }

        public ICommand NavigateCreateOwnerWindowCommand { get; }

        public CreateRegistryRecordViewModel(NavigationStore navigationStore, Action<RegistryRecord> OnCreateRegistryRecord)
        {
            _navigationStore = navigationStore;

            OnCreateRegistryRecord += OnCreateRegistryRecord;

            AddInputPairCommand = new RelayCommand(AddInputPair);

            RemoveInputPairCommand = new RelayCommand(RemoveInputPair);

            CreateRegistryRecordCommand = new RelayCommand(CreateRegistryRecord);

            NavigateCreateOwnerWindowCommand = new NavigateWindowCommand<CreateOwnerViewModel>
                (new NavigationService<CreateOwnerViewModel>(_navigationStore, (id) => new CreateOwnerViewModel(OnOwnerCreated)), () => new CreateOwnerWindow());

        }

        private void OnOwnerCreated(Owner owner)
        {
            Owners.Add(owner);

            var sorted = Owners.OrderByDescending(o => o.DateAdded).ToList();

            Owners.Clear();

            foreach( var sortedOwner in sorted)
            {
                Owners.Add(sortedOwner);
            }

            SelectedOwner = owner;


        }

        private async void CreateRegistryRecord(object sender)
        {
            try
            {
                foreach (var pair in MedInputPair)
                {
                    //Trace.WriteLine("Medicament: " + pair.Med.Name);
                    //Trace.WriteLine("Cantitate: " + pair.Quantity);
                    //Trace.WriteLine("Rank: " + pair.Rank);
                    if ((float)pair.Med.TotalAmount < pair.Quantity)
                    {
                        Boxes.ErrorBox("Cantitatea de medicament introdusă pentru " + pair.Med.Name + " este mai mică decât cea din stoc!");
                        return;
                    }

                }

                BaseRepository<RegistryRecord> registryRecordRepository = new BaseRepository<RegistryRecord>();
                BaseRepository<TreatmentMed> treatmentMedRepository = new BaseRepository<TreatmentMed>();
                BaseRepository<Med> medRepository = new BaseRepository<Med>();
                BaseRepository<Treatment> treatmentRepository = new BaseRepository<Treatment>();

                //create treatment
                var treatment = new Treatment()
                {
                    PatientId = 1,
                    OwnerId = SelectedOwner.Id,
                    PatientType = "livestock",
                    Details = "empty",
                    DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                treatment = await treatmentRepository.Add(treatment);

                if (treatment == null)
                {
                    Boxes.ErrorBox("Eroare in crearea tratamentului!");
                    return;
                }

                //create registry record
                RegistryRecord registryRecord = new RegistryRecord()
                { 
                    TreatmentId = treatment.Id,
                    Date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Species = Species,
                    Sex = Sex,
                    Age = Age,
                    Identifier = Identifier,
                    Symptoms = Symptoms,
                    RecipeDate = (int)((DateTimeOffset)RecipeDate).ToUnixTimeSeconds(),
                    RecipeNumber = RecipeNumber,
                    MedName = MedName,
                    Outcome = Outcome,
                    TreatmentDuration = TreatmentDuration,
                    Observations = Observations,
                };

                registryRecord = await registryRecordRepository.Add(registryRecord);

                //create treatment meds
                foreach (var pair in MedInputPair)
                {
                    pair.Med.TotalAmount = pair.Med.TotalAmount - pair.Quantity;
                    pair.Med.Pieces = pair.Med.TotalAmount / pair.Med.PerPiece;
                    await medRepository.Update(pair.Med);

                    TreatmentMed tretmentMed = new TreatmentMed()
                    { 
                        MedId = pair.Med.Id,
                        TreatmentId = treatment.Id,
                        Quantity = pair.Quantity,
                        Pieces = pair.Quantity / pair.Med.PerPiece,
                        Administration = pair.Administration,
                        WaitingTime = pair.WaitingTime 
                    };

                    var tm = await treatmentMedRepository.Add(tretmentMed);

                    tm.Med = pair.Med;
                    treatment.TreatmentMeds.Add(tm);
                }
                registryRecord.Treatment = treatment;
                OnCreateRegistryRecord.Invoke(registryRecord);
                Boxes.InfoBox("Tratamentul a fost adăugat în registru cu success!");

            }catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentul nu a putut fi creat!\n" + e.ToString());
            }



        }

        public void AddInputPair(object parameter)
        {
            int count = MedInputPair.Count();
            MedInputPair.Add(new MedInputPair { Med = Meds[0], Quantity = 0, Rank = count });
        }

        public void RemoveInputPair(object parameter)
        {
            try
            {
                MedInputPair.Remove(MedInputPair[(int)parameter]);

                for (int i = 0; i < MedInputPair.Count(); i++)
                {
                    MedInputPair[i].Rank = i;
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Medicamentul nu poate fi șters din listă!\n" + e.Message);
            }

        }

        public async Task LoadOwners()
        {
            BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();

            var owners = await ownerRepository.GetAll();

            var sorted = owners.OrderByDescending(o => o.DateAdded);
            
            foreach( var owner in sorted)
            {
                Owners.Add(owner);
            }

        }

        public async Task LoadMeds()
        {
            BaseRepository<Med> medRepository = new BaseRepository<Med>();

            var meds = await medRepository.GetAll();

            var sorted = meds.OrderByDescending(o => o.DateAdded);

            foreach (var med in sorted)
            {
                Meds.Add(med);
            }

            MedInputPair.Add(new Data.MedInputPair { Med = Meds[0], Quantity = 0, Rank = 0 });
        }
    }
}
