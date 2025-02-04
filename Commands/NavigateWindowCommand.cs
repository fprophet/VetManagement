using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.Commands
{
    public class NavigateWindowCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        private readonly Func<TViewModel> createViewModel;

        private readonly Window _window;

        public NavigateWindowCommand(NavigationService<TViewModel> navigationService,Window window)
        {
            _window = window;
            _navigationService = navigationService;
        }


        public override void Execute(object parameter)
        {
            _navigationService.NavigateWindow(_window, false);
        }
    }
}
