using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.DataWrappers;

namespace VetManagement.Data
{
    public class MedInputPair : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private MedWrapper _medWrapper;

        private decimal _quantity;

        private float _rank;

        public MedWrapper MedWrapper
        {
            get => _medWrapper;

            set
            {
                _medWrapper = value;
                OnPropertyChanged(nameof(MedWrapper));
                OnPropertyChanged(nameof(StockQuantity));
            }
        }

        public decimal Quantity
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
                decimal parsed;
                if(decimal.TryParse(value, out parsed))
                {
                    _quantityString = value;
                    Quantity = parsed;
                    OnPropertyChanged(nameof(QuantityString));
                }
            }
        }

        private string _administration = string.Empty;
        public string Administration
        {
            get => _administration;
            set
            {
                    _administration = value;
                    OnPropertyChanged(nameof(Administration));
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

        public decimal StockQuantity
        {
            get => MedWrapper.Med.TotalAmount;

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
