using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using VetManagement.Data;
using VetManagement.Services;

namespace VetManagement.DataWrappers
{
    public class RecipeWrapper : INotifyPropertyChanged
    {

        public RecipeWrapper (Recipe recipe)
        {
            Recipe = recipe;
        }

        public int Id
        {
            get => _recipe.Id;
            set
            {
                _recipe.RegistryNumber = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private Recipe _recipe;
        public Recipe Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                OnPropertyChanged(nameof(Recipe));
            }
        }

        public DateTime Date
        {
            get => _recipe.Date;
            set
            {
                _recipe.Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public DateTime? SignedAt
        {
            get => _recipe.SignedAt;
            set
            {
                _recipe.SignedAt = value;
                OnPropertyChanged(nameof(SignedAt));
            }
        }

        public int RegistryNumber
        {
            get => _recipe.RegistryNumber;
            set
            {
                _recipe.RegistryNumber = value;
                OnPropertyChanged(nameof(RegistryNumber));
            }
        }

        public string MedName
        {
            get => _recipe.MedName;
            set
            {
                _recipe.MedName = value;
                OnPropertyChanged(nameof(MedName));
            }
        }

        public bool Signed
        {
            get => _recipe.Signed;
            set
            {
                _recipe.Signed = value;
                OnPropertyChanged(nameof(Signed));

                if (value == true)
                {
                    SignatureImage = ImageHelper.ConvertBase64ToBitmapImage(_recipe.OwnerSignature);
                }
            }
        }

        public string OwnerSignature
        {
            get => _recipe.OwnerSignature;
            set
            {
                _recipe.OwnerSignature = value;
                OnPropertyChanged(nameof(OwnerSignature));
            }
        }

        public RegistryRecord RegistryRecord
        {
            get => _recipe.RegistryRecord;
            set
            {
                _recipe.RegistryRecord = value;
                OnPropertyChanged(nameof(RegistryRecord));
            }
        }

        private BitmapImage? _signatureImage;
        public BitmapImage? SignatureImage
        {
            get
            {
                if( _signatureImage == null)
                {
                    return ImageHelper.ConvertBase64ToBitmapImage(_recipe.OwnerSignature);
                }

                return _signatureImage;
            }

            set 
            {
                _signatureImage = ImageHelper.ConvertBase64ToBitmapImage(_recipe.OwnerSignature);
                OnPropertyChanged(nameof(SignatureImage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




    }
}
