using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VetManagement.Services;
using VetManagement.ViewModels;

namespace VetManagement.Commands
{
    class CloseWindowCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {

        private TViewModel _viewModel;

        private readonly WindowService<TViewModel> _windowService;

        public override void Execute(object parameter)
        {
            _windowService.CloseWindow(_viewModel.GetType().FullName);

        }

        public CloseWindowCommand(WindowService<TViewModel> windowService, TViewModel viewModel)

        {
            _windowService = windowService;
            _viewModel = viewModel;
            Execute(parameter: null!);

        }
    }
}
