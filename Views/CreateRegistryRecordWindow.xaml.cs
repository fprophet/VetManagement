using System;
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
using System.Windows.Shapes;
using VetManagement.ViewModels;

namespace VetManagement.Views
{
    /// <summary>
    /// Interaction logic for CreateRegistryRecordWindow.xaml
    /// </summary>
    public partial class CreateRegistryRecordWindow : Window
    {
        public CreateRegistryRecordWindow()
        {
            InitializeComponent();
            Loaded += CreateRegistryRecordWindow_Loaded;
        }

        private async void CreateRegistryRecordWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if( DataContext is CreateRegistryRecordViewModel createRegistryRecordViewModel )
            {

                await createRegistryRecordViewModel.CreateTreatmentViewModel.LoadOwners();
                await createRegistryRecordViewModel.CreateTreatmentViewModel.LoadMeds();
            }
        }
    }
}
