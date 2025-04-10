﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class OwnerPatientsViewModel : ViewModelBase
    {
        private readonly int PassedId;

        public ICommand NavigateOwnersCommand { get; set; }

        public ICommand NavigateCreatePatientWindowCommand { get; set; }

        public ICommand UpdatePatientCommand { get; set; }

        public ICommand DeletePatientCommand { get; set; }

        public Owner? Owner;

        public ObservableCollection<Patient> Patients{ get; private set; } = new ObservableCollection<Patient>();

        private object _selectedRow;
        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (value is Patient)
                {
                    SelectedPatient = (Patient)value;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        private bool _isLoading = false;
        public bool isLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(isLoading));
            }
        }


        public OwnerPatientsViewModel(NavigationStore navigationStore,int? id)
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

            OnLoadedCommand = new RelayCommand(async (object parameter) =>
            {
                await LoadOwner();
                await LoadPatients();
            });

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            NavigateCreatePatientWindowCommand = new NavigateWindowCommand<CreatePatientViewModel>
                (new WindowService<CreatePatientViewModel>(_navigationStore, (id) => new CreatePatientViewModel(_navigationStore,OnPatientCreated, id)), () => new CreatePatientWindow());

            UpdatePatientCommand = new RelayCommand(UpdatePatient, CanExecutePatientAction);
            DeletePatientCommand = new RelayCommand(DeletePatient, CanExecutePatientAction);
        }
        public bool CanExecutePatientAction(object parameter) => _selectedPatient != null && _selectedPatient.Id is int;

        private async void UpdatePatient(object parameter)
        {
            try
            {
                if (_selectedPatient != null)
                {

                    await new BaseRepository<Patient>().Update(_selectedPatient);
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
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa ștergeți acest pacient?\n Tratamentele acestui pacient vor fi arhivate!");
            if(result != MessageBoxResult.Yes)
            {
                return;
            }

            if (_selectedPatient.Id is int )
            {
                isLoading = true;

                try
                {
                    await Task.Run(async () =>
                    {
                        await PatientService.Delete(_selectedPatient);
                    });

                    Patients.Remove(_selectedPatient);
                    _selectedPatient = null;
                    Boxes.InfoBox("Pacientul a fost șters cu succes!");
                }
                catch (Exception e)
                {
                    Boxes.ErrorBox("Probleme în ștergerea medicamentului!\n" + e.Message);
                    return;
                }
                finally
                {
                    isLoading = false;
                }

                var med = Patients.FirstOrDefault(p => p.Id == _selectedPatient.Id );
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
            if(_navigationStore != null)
            {
                _navigationStore.PageTitle = "🐕 Animalele lui " + Owner.Name;
            }

        }

        public async Task LoadPatients()
        {
            isLoading = true;
            try
            {

            await Task.Run(async () =>
            {
                var patients = await new PatientRepository().GetForOwner(PassedId);

                var sortedPatients = patients.OrderByDescending(t => t.DateAdded);

                foreach (var patient in sortedPatients)
                {
                    Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Patients.Add(patient);

                    }, DispatcherPriority.Background);
                }
            });
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex.ToString());
                Boxes.ErrorBox("Lista de pacienți nu a putut fi redată.\n" + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
