using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;

namespace VetManagement.Services
{
    public class FilterHelper : INotifyPropertyChanged
    {
        private bool _areFiltersVisible = false;
        public bool AreFiltersVisible
        {
            get => _areFiltersVisible;
            set
            {
                _areFiltersVisible = value;
                if(value == false)
                {
                    FilterToggleSymbol = "▶";
                }
                else
                {
                    FilterToggleSymbol = "🔽";

                }

                OnPropertyChanged(nameof(AreFiltersVisible));
            }
        }

        private string _filterToggleSymbol = "▶";
        public string FilterToggleSymbol
        {
            get => _filterToggleSymbol;
            set
            {
                _filterToggleSymbol = value;
                OnPropertyChanged(nameof(FilterToggleSymbol));
            }
        }

        public ICommand ToggleVisibilityCommand { get; }

        public FilterHelper()
        {
            ToggleVisibilityCommand = new RelayCommand((object parameter) => AreFiltersVisible = !AreFiltersVisible);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
