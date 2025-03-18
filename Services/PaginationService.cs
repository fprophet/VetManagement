using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mysqlx.Prepare;
using VetManagement.Commands;
using VetManagement.Data;

namespace VetManagement.Services
{
    public class PaginationService : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public int PageNumber = 1;
        
        public int TotalPages = 0;

        private int _totalFound = 0;
        public int TotalFound
        {
            get => _totalFound;
            set
            {
                _totalFound = value;
                OnPropertyChanged(nameof(TotalFound));
                AddPageNumbers(TotalFound);

            }
        }


        private int _perPage = 5;
        public int PerPage
        {
            get => _perPage;
            set
            {
                _perPage = value;
                OnPropertyChanged(nameof(PerPage));
                PageNumber = 1;
                NextPageCB?.Invoke();

            }
        }

        private bool _showPagination = false;
        public bool ShowPagination
        {
            get => _showPagination;
            set
            {
                _showPagination = value;
                OnPropertyChanged(nameof(ShowPagination));

            }
        }


        public ICommand PreviousPageCommand { get; set; }

        public ICommand NextPageCommand { get; set; }

        public ICommand GoToPageCommand { get; set; }

        public bool CanGoPrevious => PageNumber > 1;

        public bool CanGoNext => PageNumber < TotalPages;

        private readonly Func<Task> PreviousPageCB;
        
        private readonly Func<Task> NextPageCB;

        public ObservableCollection<PageItem> PageNumbers { get; set; } = new ObservableCollection<PageItem>();


        public PaginationService( Func<Task> previousPage, Func<Task> nextPage, int? perPage = 5)
        {
            PreviousPageCommand = new RelayCommand(PreviousPage);
            NextPageCommand = new RelayCommand(NextPage);
            GoToPageCommand = new RelayCommand(GoToPage);

            if( perPage is not null)
            {
                PerPage = (int)perPage;
             
            }

            PreviousPageCB = previousPage;
            NextPageCB = nextPage;
            UpdateButtonState();

        }

        private async void NextPage(object sender)
        {
            if (PageNumber >= TotalPages)
            {
                return;
            }

            PageNumber++;
            await NextPageCB?.Invoke();
       
            ChangeNumberStatus();
            UpdateButtonState();

        }

        private async void PreviousPage(object sender)
        {

            if (PageNumber <= 0)
            {
                return;
            }

            PageNumber--;
            await PreviousPageCB?.Invoke();

            UpdateButtonState();
        }

        private async void GoToPage(object parameter)
        {
            if (parameter is int)
            {
                PageNumber = (int)parameter;
                await NextPageCB?.Invoke();

                UpdateButtonState();
            }
        }

        private void ChangeNumberStatus()
        {
            PageItem? prevPageItem = PageNumbers.FirstOrDefault( p => p.IsSelected == true);

            if (prevPageItem != null)
            {
                prevPageItem.IsSelected = false;
            }

            PageItem? currentPageItem = PageNumbers.FirstOrDefault(p => p.PageNumber == PageNumber);

            if (currentPageItem != null)
            {
                currentPageItem.IsSelected = true;
            }
        }

        public void AddPageNumbers(int totalRecords)
        {
            TotalPages = (int)Math.Ceiling((double)totalRecords / PerPage); 

            PageNumbers.Clear();

            if (TotalPages > 5)
            {
                for (int i = PageNumber; i <= PageNumber + 3 &&  i <= TotalPages; i++)
                {
           
                    PageNumbers.Add(new PageItem(i));
                }
            }
            else
            {
                for (int i = 1; i <= TotalPages; i++)
                {
                    PageNumbers.Add(new PageItem(i));
                }
            }

            ChangeNumberStatus();
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (TotalPages > 1)
            {
                ShowPagination = true;
            }
            else 
            {
                ShowPagination = false;
            }
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
    }
}
