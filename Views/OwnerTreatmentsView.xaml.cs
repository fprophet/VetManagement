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
    /// Interaction logic for OwnerTreatmentsView.xaml
    /// </summary>
    public partial class OwnerTreatmentsView : UserControl
    {
        public OwnerTreatmentsView()
        {
            InitializeComponent();
            Loaded += OwnerTreatmentsView_Loaded;
        }

        private async void OwnerTreatmentsView_Loaded(object sender, RoutedEventArgs e)
        {
            if( DataContext is OwnerTreatmentsViewModel ownerTreatmentsViewModel)
            {
                await ownerTreatmentsViewModel.LoadOwner();
                await ownerTreatmentsViewModel.LoadTreatments();
            }
        }

    }
}
