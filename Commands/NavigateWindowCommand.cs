﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly WindowService<TViewModel> _windowService;

        private readonly Func<Window> _window;

        public NavigateWindowCommand(WindowService<TViewModel> windowService, Func<Window> window, bool directExecution = false, bool closeCurrent = false)
        {
            _window = window;
            _windowService = windowService;

            Window? currentWindow =
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            if (directExecution)
            {
                Execute(parameter: null!);
            }

            if (closeCurrent)
            {
                currentWindow?.Close();
            }
        }


        public override void Execute(object? parameter)
        {

            if (parameter is int id)
            {
                _windowService.NavigateWindow(_window, id);
            }
            else
            {
                _windowService.NavigateWindow(_window, null);
            }
        }
    }
}
