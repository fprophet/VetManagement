using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.Commands
{
    public  class NavigateCommand<TViewModel> : RelayCommand where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;
        private readonly Predicate<object>? _canExecute;

        public NavigateCommand(NavigationService<TViewModel> navigationService, Predicate<object>? canExecute = null) : base(parameter => { }, canExecute)
        {
            _canExecute = canExecute;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate((int?)parameter);
        }
    }
}
