using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VetManagement.Services
{
    public class ImageHelper
    {
        public static BitmapImage ConvertBase64ToBitmapImage(string base64String)
        {
            if(string.IsNullOrEmpty(base64String))
            {
                return null; 
            }

            if( base64String.Contains(","))
            {
                base64String = base64String.Substring(base64String.IndexOf(",") + 1);
            }
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                return bitmap;
            }
        }
    }
}
