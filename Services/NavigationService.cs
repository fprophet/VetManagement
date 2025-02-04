 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.Services
{
    public  class NavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel = null)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void NavigateWindow(Window window,bool closeCurrent = false) 
        {
            window.Show();
        }

        public void Navigate() 
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }

    }
}
