using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities.Encoders;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class CreateOwnerAndPatientViewModel : ViewModelBase
    {

        private Action<Owner> _updateOwnersList;

        public CreatePatientViewModel CreatePatientViewModel { get; set; }

        public CreateOwnerViewModel CreateOwnerViewModel { get; set; }

        public ICommand CreateOwnerAndPatientCommand { get; set; }

        public ICommand NavigateOwnersCommand { get; set; }


        public CreateOwnerAndPatientViewModel(NavigationStore navigationStore, Action<Owner> updateOwnersList)
        {
            _navigationStore = navigationStore;
            _updateOwnersList = updateOwnersList;

            CreatePatientViewModel = new CreatePatientViewModel(null,null);
            CreateOwnerViewModel = new CreateOwnerViewModel(updateOwnersList);

            NavigateOwnersCommand = NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            CreateOwnerAndPatientCommand = new RelayCommand(CreateOwnerAndPatient);

        }


        private async void CreateOwnerAndPatient(object parameter)
        {
            try
            {
                Patient patient = CreatePatientViewModel.RetrievePatient();
                Owner owner= CreateOwnerViewModel.RetrieveOwner();

                if (owner == null || patient == null) 
                {
                    return;
                }

                BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();
                BaseRepository<Patient> patientRepository = new BaseRepository<Patient>();

                owner = await ownerRepository.Add(owner);

                patient.OwnerId = owner.Id;

                await patientRepository.Add(patient);

                _updateOwnersList?.Invoke(owner);
                Boxes.InfoBox("Pacientul a fost adăugat cu succes!");

            }
            catch (Exception ex)
            {
                Boxes.ErrorBox("Pacientul nu a putut fi înregistrat!\n" + ex.Message);
                Logger.LogError("Error", ex.ToString());

            }

        }
     
    }
}
