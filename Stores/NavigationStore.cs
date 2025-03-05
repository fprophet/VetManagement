using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VetManagement.ViewModels;

namespace VetManagement.Stores
{
    public class NavigationStore
    {

        public event Action? CurrentViewModelChanged;

        public event Action? ItemIdChanged;

        public event Action? PageTitleChanged;

        private ViewModelBase _currentViewModel;

        public readonly Dictionary<Window, string> Windows = new Dictionary<Window, string>();

        public string PageTitle
        {
            get => _currentViewModel.PageTitle;
            set
            {
                if (_currentViewModel != null)
                {
                    _currentViewModel.PageTitle = value;
                    PageTitleChanged?.Invoke();
                }
            }

        }

        private int _passedId;
        public int PassedId
        {
            get => _passedId;
            set
            {
                if (_passedId != null)
                {
                    _passedId = value;
                }
            }

        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();

            }

        }

        private void OnCurrentViewModelChanged()

        {
            CurrentViewModelChanged?.Invoke();

        }

        private void OnPageTitleChanged()
        {
            Trace.WriteLine(_currentViewModel.PageTitle);
            PageTitleChanged?.Invoke();
        }
    }
}
