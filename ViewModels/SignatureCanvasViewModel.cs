using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using VetManagement.Commands;
using VetManagement.Services;

namespace VetManagement.ViewModels
{
    public class SignatureCanvasViewModel : ViewModelBase
    {
        private StrokeCollection _signatureStrokes;
        public StrokeCollection SignatureStrokes
        {
            get => _signatureStrokes;

            set
            {
                _signatureStrokes = value;
                OnPropertyChanged(nameof(SignatureStrokes));
            }
        }


        public ICommand ClearCommand { get; }
        public ICommand SaveCommand { get; }

        public SignatureCanvasViewModel()
        {
            SignatureStrokes = new StrokeCollection();

            ClearCommand = new RelayCommand(ClearStrokes);
            SaveCommand = new RelayCommand(SaveStrokes);
        }

        private void ClearStrokes(object sender)
        {
            SignatureStrokes.Clear();
        }

        private void SaveStrokes(object sender)
        {

            bool IsTabletConnected = System.Windows.Input.Tablet.TabletDevices.Count > 0;
            if (IsTabletConnected)
            {
                Boxes.InfoBox("Tablet detected: Ready for signature input.");
            }
            else
            {
                Boxes.InfoBox("NO tablet detectd.");

            }

            return;
            if ( SignatureStrokes.Count == 0 )
            {
                Boxes.InfoBox("Semnătura este goală!");
                return;
            }


            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Semnătură"
            };

            if( saveFileDialog.ShowDialog() == true)
            {
                SaveSignature(saveFileDialog.FileName);
                Boxes.InfoBox("Semnătura a fost salvată!");

            }
        }

        private void SaveSignature( string fileName)
        {
            int width = 500, height = 200;

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
                SignatureStrokes.Draw(dc);
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                encoder.Save(fs);
            }
        }

    }
}
