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
    /// Interaction logic for TreatmentsView.xaml
    /// </summary>
    public partial class TreatmentsView : UserControl
    {
        public TreatmentsView()
        {
            InitializeComponent();
            Loaded += TreatmentsView_Loaded;
        }

        private async void TreatmentsView_Loaded(object sender, RoutedEventArgs e)
        {
            if( DataContext is TreatmentsViewModel treatmentsView)
            {
                await treatmentsView.LoadTreatments();
            }
        }
    }
}
