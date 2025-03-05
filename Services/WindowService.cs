using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.Services
{
    public class WindowService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        
        private readonly Func<int?, TViewModel> _createViewModel;


        public WindowService(NavigationStore navigationStore, Func<int?, TViewModel> createViewModel)
        {
         
            _createViewModel = createViewModel;
            _navigationStore = navigationStore;
        }

        public void RegisterWindow(Window window, string viewModel )
        {
            _navigationStore.Windows[window] = viewModel;
            window.Closing += (s, e) => UnregisterWindow(window);
        }

        public void UnregisterWindow(Window window)
        {
            _navigationStore.Windows.Remove(window);
        }

        public Window? GetWindowByViewModel(string viewModel)
        {
            return _navigationStore.Windows.FirstOrDefault(kv => kv.Value == viewModel).Key;
        }

        public void NavigateWindow(Func<Window> createWindow, int? passedId)
        {
            if (passedId != null)
            {
                _navigationStore.PassedId = (int)passedId;
            }

            Window window = createWindow();
            window.DataContext = _createViewModel(_navigationStore.PassedId);

            RegisterWindow(window, window.DataContext.GetType().FullName);
  
            window.Show();
        }


        public void CloseWindow(string ViewModelName)
        {
            Window window = GetWindowByViewModel(ViewModelName);

            if( window == null )
            {
                return;
            }

            UnregisterWindow(window);
          
            window.Close();
        }
    }
}
