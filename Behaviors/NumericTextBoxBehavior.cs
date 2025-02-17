using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
namespace VetManagement.Behaviors
{
    public class NumericTextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Trace.WriteLine("Attached!");

            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.TextChanged += OnTextChanged;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            AssociatedObject.TextChanged -= OnTextChanged;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            // This regular expression allows a number with an optional decimal part
            if (!Regex.IsMatch(newText, @"^\d*\.?\d*$"))
            {
                Trace.WriteLine("Invalid");
                Trace.WriteLine(newText);
                e.Handled = true; // Block invalid input
            }
            Trace.WriteLine("Valid");
            Trace.WriteLine(newText);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Replace comma with dot for consistency
            textBox.Text = textBox.Text.Replace(',', '.');

            // Move caret to the end to prevent cursor jump
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.Key == Key.OemPeriod) // Decimal point
            {
                Trace.WriteLine("Pula");
                Trace.WriteLine(e.Key);
                // Prevent the key from being processed
                e.Handled = false;
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string pasteText = e.DataObject.GetData(DataFormats.Text) as string;
                if (!Regex.IsMatch(pasteText, @"^\d*\.?\d*$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}