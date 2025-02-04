using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using VetManagement.Commands;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;

        public ICommand NavigateHomeCommand { get; }


        public InventoryViewModel(NavigationStore navigationStore) 
        { 
            _navigationStore = navigationStore;
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, () => new HomeViewModel(_navigationStore)));

        }

    }
}
