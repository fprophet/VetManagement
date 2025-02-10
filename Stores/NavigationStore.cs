using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.ViewModels;

namespace VetManagement.Stores
{
    public class NavigationStore
    {

        public event Action CurrentViewModelChanged;

        public event Action ItemIdChanged;

        public event Action PageTitleChanged;

        private ViewModelBase _currentViewModel;

        public string PageTitle
        {
            get => _currentViewModel.PageTitle;
            set
            {
                _currentViewModel.PageTitle = value;
                OnPageTitleChanged();
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
            Trace.WriteLine("Aici:");
            Trace.WriteLine(_currentViewModel);
            Trace.WriteLine(_currentViewModel.PageTitle);
            PageTitleChanged?.Invoke();
        }
    }
}
