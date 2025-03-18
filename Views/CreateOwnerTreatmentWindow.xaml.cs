using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VetManagement.ViewModels;

namespace VetManagement.Views
{
    /// <summary>
    /// Interaction logic for CreateTreatmentWindow.xaml
    /// </summary>
    public partial class CreateOwnerTreatmentWindow : Window
    {
        public CreateOwnerTreatmentWindow()
        {
            InitializeComponent();
            Loaded += CreateTreatmentView_Loaded;
        }

        private async void CreateTreatmentView_Loaded(object sender, RoutedEventArgs e)
        {
  
            if (DataContext is CreateTreatmentViewModel createTreatmentViewModel)
            {
                await createTreatmentViewModel.LoadOwner();
                await createTreatmentViewModel.LoadOwnerPatients(createTreatmentViewModel.Owner.Id);
            }
        }

    }
}
