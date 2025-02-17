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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VetManagement.ViewModels;

namespace VetManagement.Views
{
    /// <summary>
    /// Interaction logic for OwnerPatientsView.xaml
    /// </summary>
    public partial class OwnerPatientsView : UserControl
    {
        public OwnerPatientsView()
        {
            InitializeComponent();
            Loaded += OwnerPatientsView_Loaded;
        }

        private async void OwnerPatientsView_Loaded(object sender, RoutedEventArgs e)
        {
            if( DataContext is OwnerPatientsViewModel ownerPatientsViewModel)
            {
                await ownerPatientsViewModel.LoadOwner();
                await ownerPatientsViewModel.LoadPatients();
            }
        }
    }
}
