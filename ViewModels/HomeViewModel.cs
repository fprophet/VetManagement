﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using VetManagement.Commands;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        public ViewModelBase CurrentViewModel { get; }

        public ICommand NavigateUsersCommand { get; }

        public ICommand NavigateInventoryCommand { get; }

        public ICommand NavigateCreateTreatmentCommand { get; }

        public ICommand NavigateOwnersCommand { get; }

        public HomeViewModel( NavigationStore navigationStore)
        {
            NavigateUsersCommand = new NavigateCommand<UsersViewModel>(new NavigationService<UsersViewModel>(navigationStore,(id) => new UsersViewModel(navigationStore)));
            NavigateInventoryCommand = new NavigateCommand<InventoryViewModel>(new NavigationService<InventoryViewModel>(navigationStore,(id) => new InventoryViewModel(navigationStore)));
            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>(new NavigationService<OwnersViewModel>(navigationStore,(id) => new OwnersViewModel(navigationStore)));
        }


    }
}
