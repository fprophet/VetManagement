using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class OwnerPatientsViewModel : ViewModelBase
    {
        private readonly int PassedId;

        private readonly string _pageTitle;

        public ICommand NavigateOwnersCommand { get; set; }

        public ICommand NavigateCreatePatientWindowCommand { get; set; }

        public ICommand UpdatePatientCommand { get; set; }

        public ICommand DeletePatientCommand { get; set; }

        public Owner Owner;

        public ObservableCollection<Patient> Patients{ get; private set; } = new ObservableCollection<Patient>();

        public OwnerPatientsViewModel(NavigationStore navigationStore, int? id)
        {

            if (id.HasValue)
            {
                PassedId = id.Value;
            }
            else
            {
                PassedId = -1;
            }

            _navigationStore = navigationStore;
            _navigationStore.PassedId = PassedId;

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            NavigateCreatePatientWindowCommand = new NavigateWindowCommand<CreatePatientViewModel>
                (new NavigationService<CreatePatientViewModel>(_navigationStore, (id) => new CreatePatientViewModel(OnPatientCreated, id)), () => new CreatePatientWindow());

            UpdatePatientCommand = new RelayCommand(UpdatePatient);
            DeletePatientCommand = new RelayCommand(DeletePatient);
        }


        private async void UpdatePatient(object parameter)
        {
            try
            {
                if (parameter != null && parameter is Patient)
                {
                    Patient patient = (Patient)parameter;
                    await new BaseRepository<Patient>().Update(patient);
                    Boxes.InfoBox("Pacientul a fost actualizat!");

                }

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare in actualizarea medicamentului!\n" + e.Message);
            }
        }

        private async void DeletePatient(object parameter)
        {
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa ștergeți acest pacientul?");

            if (parameter is int && result == MessageBoxResult.Yes)
            {
                try
                {
                    await new BaseRepository<Patient>().Delete((int)parameter);
                }
                catch (Exception e)
                {
                    Boxes.ErrorBox("Probleme în ștergerea medicamentului!\n" + e.Message);
                    return;
                }

                var med = Patients.FirstOrDefault(p => p.Id == (int)parameter);
                if (med != null)
                {
                    Patients.Remove(med);
                }
            }
            else
            {
                Boxes.ErrorBox("Probleme în ștergerea pacientului! No ID found!");
            }
        }


        private void OnPatientCreated(Patient patient)
        {
            Patients.Add(patient);
            var sortedTreatments = Patients.OrderByDescending(t => t.DateAdded).ToList();

            Patients.Clear();

            foreach (var sorted in sortedTreatments)
            {
                Patients.Add(sorted);
            }
        }


        public async Task LoadOwner()
        {
            Owner = await new BaseRepository<Owner>().GetById(PassedId);
            _navigationStore.PageTitle = "Animalele lui " + Owner.Name;


        }

        public async Task LoadPatients()
        {
            var patients = await new PatientRepository().GetForOwner(PassedId);

            var sortedPatients = patients.OrderByDescending(t => t.DateAdded);

            foreach (var patient in sortedPatients)
            {
                Patients.Add(patient);
            }


        }
    }
}
