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
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : UserControl
    {
        public RecipeView()
        {
            InitializeComponent();
            Loaded += RecipeView_Loaded;
        }

        private async void RecipeView_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is RecipeViewModel recipeViewModel)
            {
                await recipeViewModel.LoadRecipe();
            }
        }
    }
}
