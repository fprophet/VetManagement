using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MedViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;
        private readonly BaseRepository<Med> _medRepository = new BaseRepository<Med>();

        public ICommand NavigateInventoryCommand { get; }

        private Med _med;

        private string _name;

        public string Name 
        {   get => _name;
            set 
            { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }

        public MedViewModel(NavigationStore navigationStore,int? id)
        {
            _navigationStore = navigationStore;
            NavigateInventoryCommand = new NavigateCommand<InventoryViewModel>
                (new NavigationService<InventoryViewModel>(navigationStore, (id) => new InventoryViewModel(navigationStore)));

            LoadMed(id);

        }

        private async void LoadMed(int? id)
        {
            try 
            {
                if (id is int)
                {
                    _med =  await _medRepository.GetById(id ?? 0);
                    _name = _med.Name;
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Medicamentul nu a putut fi găsit!\n" + ex.Message, "Error", MessageBoxButton.OK);

            }


        }
    }
}
