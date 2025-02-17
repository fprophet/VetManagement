using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{


    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string PageTitle { get; set; }

        protected NavigationStore _navigationStore;

        bool INotifyDataErrorInfo.HasErrors => Errors.Any();

        public Dictionary<string, List<string>> Errors = new();


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public  virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        protected void OnErrorsChanged(string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
