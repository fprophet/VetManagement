using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ICSharpCode.SharpZipLib.Core;
using NPOI.HSSF.UserModel;
using OfficeOpenXml;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Migrations;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class ImportedProductsViewModel : ViewModelBase
    {
        public ICommand ImportProductsCommand { get; }
        public ICommand BrowseFileCommand { get; }

        private readonly ImportedProductsFileHelper _importedProductsFileHelper;

        public ObservableCollection<ImportedProduct> ImportedProducts { get; set; } = new ObservableCollection<ImportedProduct>();

        public PaginationService PaginationService { get; }

        private bool _isLoading = true;
        public bool isLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(isLoading));
            }
        }

        private int _totalProducts;
        public int TotalProducts
        {
            get => _totalProducts;
            set
            {
                _totalProducts = value;
                OnPropertyChanged(nameof(TotalProducts));
            }
        }

        private int _currentProductIndex;
        public int CurrentProductIndex
        {
            get => _currentProductIndex;
            set
            {
                _currentProductIndex = value;
                OnPropertyChanged(nameof(CurrentProductIndex));

            }
        }

        private string _currentProductName;
        public string CurrentProductName
        {
            get => _currentProductName;
            set
            {
                _currentProductName = value;
                OnPropertyChanged(nameof(CurrentProductName));

            }
        }    
        
        private float _progressFiller;
        public float ProgressFiller
        {
            get => _progressFiller;
            set
            {
                _progressFiller = value;
                OnPropertyChanged(nameof(ProgressFiller));
            }
        }

        private bool _isProgressVisible = false;
        public bool IsProgressVisible
        {
            get => _isProgressVisible;
            set
            {
                _isProgressVisible = value;
                OnPropertyChanged(nameof(IsProgressVisible));
            }
        } 
        
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public ImportedProductsViewModel(NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📦 Produse Importate";
            _importedProductsFileHelper = new ImportedProductsFileHelper(UpdateProgress);

            PaginationService = new PaginationService(() => LoadImportedProducts(), () => LoadImportedProducts(),20);


            ImportProductsCommand = new RelayCommand(ImportProducts, (object parameter) => !string.IsNullOrEmpty(FilePath) && !IsProgressVisible);

            BrowseFileCommand = new RelayCommand(BrowseFile);
        }

        private void BrowseFile(object parameter)
        {
            FilePath = BrowseFileHelper.BrowseExcelFile();
        }

        private void UpdateProgress(int totalProducts, int currentProductIndex, string currentProductName)
        {
            //Trace.WriteLine(CurrentProductIndex);

            TotalProducts = totalProducts;
            CurrentProductIndex = currentProductIndex;
            CurrentProductName = currentProductName;

            ProgressFiller = ((float)CurrentProductIndex / TotalProducts) * 100;

        }

        private async void ImportProducts(object parameter)
        {
            IsProgressVisible = true;
            try
            {
                bool res = await _importedProductsFileHelper.ImportProducts(FilePath);
                if(res)
                {
                    await LoadImportedProducts();
                    Boxes.InfoBox("Produsele au fost importate cu succes!");
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare in citirea fișierului!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
            finally
            {
                IsProgressVisible = false;

            }

        }



        public async Task LoadImportedProducts()
        {
            isLoading = true;
            ImportedProducts.Clear();
            try
            {

                var (importedProducts, totalRecords) = await new ImportedProductRepository().GetAllFiltered(PaginationService.PageNumber, PaginationService.PerPage, null);

                PaginationService.TotalFound = totalRecords;

                foreach (var importedProduct in importedProducts)
                {
                    ImportedProducts.Add(importedProduct);
                }
               
            }

            catch (Exception e)
            {
                MessageBox.Show("Lista de produse importate nu a putut fi redată!\n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isLoading = false;
            }


        }
    }
}
