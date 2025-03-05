using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using System.Diagnostics;

namespace VetManagement.DataWrappers
{
    public class MedWrapper : INotifyPropertyChanged
    {
        private Med _med;

        public Med Med
        {
            get => _med;
            set 
            {
                _med = value;
                OnPropertyChanged(nameof(Med));

                NotifyAllPropertiesChanged();

            }
        }

        public int Id
        {
            get => _med.Id;
            set
            {
                _med.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _med.Name;
            set
            {
                _med.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Type
        {
            get => _med.Type;
            set
            {
                _med.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string MeasurmentUnit
        {
            get => _med.MeasurmentUnit;
            set
            {
                _med.MeasurmentUnit = value;
                OnPropertyChanged(nameof(MeasurmentUnit));
            }
        }


        public string PieceType
        {
            get => _med.PieceType;
            set
            {
                _med.PieceType = value;
                OnPropertyChanged(nameof(PieceType));
                //CalculateTotalAmount();

            }
        }

        private string _perPieceString = String.Empty;
        public string PerPieceString
        {
            get => _perPieceString;
            set
            {
                decimal parsed;
                if (decimal.TryParse(value, out parsed))
                {
                    _perPieceString = value;
                    PerPiece = parsed;
                    OnPropertyChanged(nameof(PerPieceString));
                }
            }
        }

        public decimal PerPiece
        {
            get => _med.PerPiece;
            set
            {
                _med.PerPiece = value;
                OnPropertyChanged(nameof(PerPiece));
                CalculateTotalAmount();
            }
        }

        public int Pieces
        {
            get => _med.Pieces;

            set
            {
                _med.Pieces = value;
                OnPropertyChanged(nameof(Pieces));
                CalculateTotalAmount();
            }
        }

        public string Provider
        {
            get => _med.Provider;
            set
            {
                _med.Provider = value;
                OnPropertyChanged(nameof(Provider));
            }
        }

        public int InvoiceNumber
        {
            get => _med.InvoiceNumber;
            set
            {
                _med.InvoiceNumber = value;
                OnPropertyChanged(nameof(InvoiceNumber));
            }
        }

        public string WaitingTime
        {
            get => _med.WaitingTime;
            set
            {
                _med.WaitingTime = value;
                OnPropertyChanged(nameof(WaitingTime));
            }
        }

        public decimal TotalAmount
        {
            get => _med.TotalAmount;
            set
            {
                _med.TotalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public string? LotID
        {
            get => _med.LotID;
            set
            {
                _med.LotID = value;
                OnPropertyChanged(nameof(LotID));
            }
        }

        public long Valability
        {
            get => _med.Valability;
            set
            {
                _med.Valability = value;
                OnPropertyChanged(nameof(Valability));
            }
        }

        public string? Description
        {
            get => _med.Description;
            set
            {
                _med.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public long DateAdded
        {
            get => _med.DateAdded;
            set
            {
                _med.DateAdded = value;
                OnPropertyChanged(nameof(DateAdded));
            }
        }

        public long DateUpdated
        {
            get => _med.DateUpdated;
            set
            {
                _med.DateUpdated = value;
                OnPropertyChanged(nameof(DateUpdated));
            }
        }

        private decimal _treatmentQuantity;
        public decimal TreatmentQuantity
        {
            get => _treatmentQuantity;
            set
            {
                _treatmentQuantity = value;
                OnPropertyChanged(nameof(TreatmentQuantity));
            }
        }

        private string _quantityString;
        public string QuantityString
        {
            get => _quantityString;
            set
            {
                decimal parsed;
                if (decimal.TryParse(value, out parsed))
                {
                    _quantityString = value;
                    TreatmentQuantity = parsed;
                    OnPropertyChanged(nameof(QuantityString));
                }
            }
        }

        public string Unit
        {
            get => _med.Unit;
        }

        //public string UnitPerPiece => PieceType == "comprimate" ? "-" : PerPiece + "ml/flacon";

        public string UnitPerPiece
        {
            get => _med.UnitPerPiece;
        }

        public string PerPieceAndUnit
        {
            get => _med.PerPieceAndUnit;
        }

        public string SingularPieceType
        {
            get => _med.SingularPieceType;
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = Pieces * PerPiece;
        }

        public MedWrapper(Med med)
        {
            _med = med;
        }

        public DateTime DateAddedFormated => DateTimeOffset.FromUnixTimeSeconds(DateAdded).UtcDateTime;

        public string ValabilityFormated =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(Valability).UtcDateTime, TimeZoneInfo.Local).Date.ToString("yyyy-MM-dd");


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void NotifyAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

    }

}
