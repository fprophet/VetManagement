 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.Services
{
    public class NavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<int?, TViewModel> _createViewModel;

        public int PassedId
        {
            get => _navigationStore.PassedId;
            set { }
        }

        public NavigationService(NavigationStore navigationStore, Func<int?, TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel ?? throw new ArgumentNullException(nameof(createViewModel));
        }

        public void Navigate(int? id = null)
        {
            var viewModel = _createViewModel(id);

            //avoid reloading page. reloading page this way does not trigger the window load event therefore not loading data
            //if (_navigationStore.CurrentViewModel.GetType().Name != viewModel.GetType().Name || renewView)
            //{
            _navigationStore.CurrentViewModel = null;
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _navigationStore.CurrentViewModel = viewModel;

            }, DispatcherPriority.Background);  
            //}
        }
    }
}
