using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class RegistryViewModel : ViewModelBase
    {

        public ObservableCollection<RegistryRecord> RegistryRecords { get; } = new ObservableCollection<RegistryRecord>();

        public ICommand NavigateCreateRegisterRecordWindowCommand { get; set; }
        public RegistryViewModel(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "Registru animale mari";

            NavigateCreateRegisterRecordWindowCommand = new NavigateWindowCommand<CreateRegistryRecordViewModel>
                (new NavigationService<CreateRegistryRecordViewModel>
                    (_navigationStore, (id) => new CreateRegistryRecordViewModel(_navigationStore, UpdateRegistriRecords)), () => new CreateRegistryRecordWindow());
        }
        

        private void UpdateRegistriRecords(RegistryRecord registryRecord)
        {
            RegistryRecords.Add(registryRecord);

            var sorted = RegistryRecords.OrderByDescending(rr => rr.Date);

            RegistryRecords.Clear();

            foreach( var rr in sorted)
            {
                RegistryRecords.Add(rr);
            }

        }

        public async Task LoadRegistryRecords()
        {
            try
            {
                RegistryRecordRepository registryRecordRepository = new RegistryRecordRepository();

                var registryRecords = await registryRecordRepository.GetRegistryRecords();

                var sorted = registryRecords.OrderByDescending(r => r.Date);

                foreach( RegistryRecord registryRecord in sorted)
                {
                    RegistryRecords.Add(registryRecord);
                }


            }catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au fost găsite!\n" + e.Message);
            }

        }
      
    }
}
