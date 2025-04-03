using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using VetManagement.ViewModels;
using VetManagement.Components.Menus;
using System.Diagnostics;

namespace VetManagement.Services
{
    public class ViewModelDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate TreatmentsMenu { get; set; }
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate InventoryMenu { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if( item is ViewModelBase)
            {
                switch( item.GetType().Name)
                {
                    case "TreatmentsViewModel":
                        return TreatmentsMenu;
                    case "InventoryViewModel":
                        return InventoryMenu;
                }
            }

            // Return a default (empty) template or null if no match
            return DefaultTemplate;
        }
    }
}
