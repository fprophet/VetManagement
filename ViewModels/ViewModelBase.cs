﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{


    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public ICommand OnLoadedCommand { get; protected set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string PageTitle { get; set; }

        protected NavigationStore? _navigationStore;


        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        bool INotifyDataErrorInfo.HasErrors => Errors.Any();

        public Dictionary<string, List<string>> Errors = new();
        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

    }
}
