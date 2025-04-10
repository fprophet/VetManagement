﻿
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class CreateRegistryRecordViewModel : ViewModelBase
    {

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

        private Treatment? _treatment;

        private Action<RegistryRecord> OnCreateRegistryRecord;
        public ICommand CreateRegistryRecordCommand { get; }

        public CreateRegistryRecordTreatmentViewModel CreateRegistryRecordTreatmentViewModel { get; } 

        public CreateRegistryRecordViewModel(NavigationStore navigationStore, Action<RegistryRecord> onCreateRegistryRecord)
        {
            _navigationStore = navigationStore;
            OnCreateRegistryRecord += onCreateRegistryRecord;

            CreateRegistryRecordTreatmentViewModel = new CreateRegistryRecordTreatmentViewModel(_navigationStore,OnTreatmentCreated, null);

            CreateRegistryRecordCommand = new RelayCommand(CreateRegistryRecord);
        }

  
        private void OnTreatmentCreated(Treatment treatment)
        {
            _treatment = treatment;
        }

        private bool Validate(RegistryRecord registryRecord)
        {

            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(registryRecord, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(registryRecord, context, validationResults);


            if (!isValid )
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

        private async void CreateRegistryRecord(object sender)
        {
            try
            {

                RegistryRecord registryRecord = new RegistryRecord()
                {
                    //TreatmentId = _treatment.Id,
                    Date = DateTime.Now,
                    //Symptoms = Symptoms,
                    //RecipeDate = (int)((DateTimeOffset)RecipeDate).ToUnixTimeSeconds(),
                    MedName = MedName,
                    Outcome = Outcome,
                    TreatmentDuration = TreatmentDuration,
                    Observations = Observations,
                };

                if (!Validate(registryRecord))
                {
                    string message = "Completați câmpurile necesare pentru Registru!\n";

                    message += string.Join("\n", Errors.Select(kv => kv.Value.FirstOrDefault())) + "\n";

                    Boxes.InfoBox(message);
                    Errors.Clear();
                    return;
                }

                bool treatmentCreated = await CreateRegistryRecordTreatmentViewModel.CreateTreatment();

                if (!treatmentCreated)
                {
                    //Boxes.ErrorBox("Eroare in crearea tratamentului!");
                    return;
                }

                registryRecord.TreatmentId = _treatment.Id;

                registryRecord = await new BaseRepository<RegistryRecord>().Add(registryRecord);


                if (registryRecord == null)
                {
                    return;

                }

                Recipe recipe = await new BaseRepository<Recipe>().Add(new Recipe()
                {
                    RegistryNumber = registryRecord.Id,
                    MedName = MedName,
                    //TreatmentId = _treatment.Id,
                    Signed = false,
                    OwnerSignature = "",
                    Date = DateTime.Now
                });


                registryRecord.RecipeNumber = recipe.Id;
                await new BaseRepository<RegistryRecord>().Update(registryRecord);

                registryRecord.Treatment = _treatment;

                OnCreateRegistryRecord?.Invoke(registryRecord);
                var res = Boxes.InfoBox("Tratamentul a fost adăugat în registru cu success!");

              
                if (res == MessageBoxResult.OK)
                {
                    new CloseWindowCommand<CreateRegistryRecordViewModel>
                        (new WindowService<CreateRegistryRecordViewModel>(_navigationStore, null), this);
                }

            }
            catch(Exception e)
            {
                Logger.LogError("Error", e.ToString());
                Boxes.ErrorBox("Tratamentul nu a putut fi creat!\n" + e.Message);
            }

        }

        private void SendRecipeNotification(int id)
        {
            Notification Notification = new Notification()
            {
                Type = "new-recipe",
                Title = "A fost creată rețeta cu numărul:" + id,
                Message = "",
                SentAt = DateTime.Now,
                UserType = "user"
            };

            NotificationService.SendNotification(Notification);
        }

    }
}
