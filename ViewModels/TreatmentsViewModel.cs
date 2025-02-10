﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class TreatmentsViewModel : ViewModelBase
    {
        public string _pageTitle = "Tratamente";

        public ICommand NavigateOwnersCommand { get; set; }

        private TreatmentRepository _treatmentRepository;

        //public CreateTreatmentViewModel CreateTreatmentViewModel { get; set; }

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();

        public TreatmentsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _treatmentRepository = new TreatmentRepository();
            _navigationStore.PageTitle = _pageTitle;
            NavigateOwnersCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, (id) => new HomeViewModel(_navigationStore)));
            LoadTreatments();
            //CreateTreatmentViewModel = new CreateTreatmentViewModel(OnTreatmentCreated, id);

        }

        private void OnTreatmentCreated(Treatment treatment)
        {
            Treatments.Add(treatment);
            var sortedTreatments = Treatments.OrderByDescending(t => t.DateAdded).ToList();

            Treatments.Clear();

            foreach (var sorted in sortedTreatments)
            {
                Treatments.Add(sorted);
            }
        }


        private async void LoadTreatments()
        {
            var treatments = await _treatmentRepository.GetFullTreatments();

            var sortedTreatments = treatments.OrderByDescending(t => t.DateAdded);

            foreach (var treatment in sortedTreatments)
            {

                Treatments.Add(treatment);
            }


        }
    }
}
