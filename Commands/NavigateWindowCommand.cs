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

        private readonly Func<Window> _window;

        public NavigateWindowCommand(NavigationService<TViewModel> navigationService, Func<Window> window)
        {
            _window = window;
            _navigationService = navigationService;
        }


        public override void Execute(object parameter)
        {
            if (parameter is int id)
            {
                _navigationService.NavigateWindow(_window, id);
            }
            else {
                _navigationService.NavigateWindow(_window, null);

            }
        }
    }
}
