﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VetManagement.ViewModels;

namespace VetManagement.Views
{
    /// <summary>
    /// Interaction logic for ImportedProductsView.xaml
    /// </summary>
    public partial class ImportedProductsView : UserControl
    {
        public ImportedProductsView()
        {
            InitializeComponent();
            Loaded += ImportedProductsView_Loaded; ;
        }

        private async void ImportedProductsView_Loaded(object sender, RoutedEventArgs e)
        {
            if( DataContext is ImportedProductsViewModel importedProductsViewModel)
            {
                await importedProductsViewModel.LoadImportedProducts();
            }
        }
    }
}
