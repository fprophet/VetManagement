using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class MedInputPair : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Med _med;

        private float _quantity;

        private float _rank;

        private float _stockQuantity;

        public Med Med
        {
            get => _med;

            set
            {
                _med = value;
                OnPropertyChanged(nameof(Med));
                OnPropertyChanged(nameof(StockQuantity));
            }
        }

        public float Quantity
        {
            get => _quantity;

            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }

        }

        private string _quantityString;
        public string QuantityString
        {
            get => _quantityString;
            set
            {
                float parsed;
                if( float.TryParse(value, out parsed))
                {
                    _quantityString = value;
                    Quantity = parsed;
                    OnPropertyChanged(nameof(QuantityString));
                }
            }
        }

        private string _administration;
        public string Administration
        {
            get => _administration;
            set
            {
                    _administration = value;
                    OnPropertyChanged(nameof(Administration));
            }
        }

        private string _waitingTime;
        public string WaitingTime
        {
            get => _waitingTime;
            set
            {
                _waitingTime = value;
                OnPropertyChanged(nameof(WaitingTime));
            }
        }


        public float Rank
        {
            get => _rank;

            set
            {
                _rank = value;
                OnPropertyChanged(nameof(Rank));
            }

        }

        public float StockQuantity
        {
            get => Med.TotalAmount;

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
