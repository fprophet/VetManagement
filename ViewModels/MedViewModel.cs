﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MedViewModel : ViewModelBase
    {

        private new readonly NavigationStore _navigationStore;

        private readonly int _passedId;

        private readonly string _pageTitle;

        private bool _hasStartedPiece = false;

        public bool HasStartedPiece
        {
            get => _hasStartedPiece;

            set
            {
                _hasStartedPiece = value;
                OnPropertyChanged(nameof(HasStartedPiece));
            }
        }

        public ObservableCollection<TreatmentMed> Treatments { get; set; } = new ObservableCollection<TreatmentMed>();

        private Med _med;
        public Med CurrentMed 
        { 
            get => _med;
            set
            {
                _med = value;
                OnPropertyChanged(nameof(CurrentMed));
            }
        }

        private decimal _startedPieceAmount;
        public decimal StartedPieceAmount 
        { 
            get => _startedPieceAmount; 
            set
            {
                _startedPieceAmount = value;
                OnPropertyChanged(nameof(StartedPieceAmount));
            }
        }

        private decimal _pieces;
        public decimal Pieces
        {
            get => _pieces;
            set
            {
                _pieces = value;
                OnPropertyChanged(nameof(Pieces));
            }
        }

        public ICommand NavigateInventoryCommand { get; }


        private string _name;

        public string Name 
        {   get => _name;
            set 
            { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }

        public MedViewModel(NavigationStore navigationStore,int? id)
        {
            _navigationStore = navigationStore;

            NavigateInventoryCommand = new NavigateCommand<InventoryViewModel>
                (new NavigationService<InventoryViewModel>(navigationStore, (id) => new InventoryViewModel(navigationStore)));

            if (id.HasValue)
            {
                _passedId = id.Value;
            }
            else
            {
                _passedId = -1;
            }
         

        }

        public async Task LoadMedTreatments()
        {
            TreatmentMedRepository treatmentMedRepository = new();

            var treatments = await treatmentMedRepository.GetTreatmentsForMed(_passedId);

            var sortedTreatments = treatments.OrderByDescending(t => t.Treatment.DateAdded);

            foreach( var treatment in sortedTreatments)
            {
                Treatments.Add(treatment);

            }
        }

        public async Task LoadMed()
        {
            try 
            {
                _med =  await new BaseRepository<Med>().GetById(_passedId);
                _navigationStore.PageTitle = "Tratamente cu: " + _med.Name;
                CurrentMed = _med;

                StartedPieceAmount = CurrentMed.Pieces % 1;
                if(StartedPieceAmount > 0)
                {
                    HasStartedPiece = true;
                    Pieces = CurrentMed.Pieces - StartedPieceAmount + 1;
                    StartedPieceAmount = decimal.Round((StartedPieceAmount / CurrentMed.PerPiece) * 100, 1);
                }
                else
                {
                    Pieces = CurrentMed.Pieces;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Medicamentul nu a putut fi găsit!\n" + ex.Message, "Error", MessageBoxButton.OK);

            }


        }
    }
}
