using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;

namespace VetManagement.DataWrappers
{
    public class ImportedMedWrapper : INotifyPropertyChanged
    {
        private ImportedMed _importedMed;

        public ImportedMed ImportedMed
        {
            get
            {
                return _importedMed;
            }
        }

        public ImportedMedWrapper(ImportedMed importedMed)
        {
            _importedMed = importedMed;
        }

        public string Id
        {
            get
            {
                return _importedMed.Id;
            }
            set
            {
                _importedMed.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Cod
        {
            get
            {
                return _importedMed.Cod;
            }
            set
            {
                _importedMed.Cod = value;
                OnPropertyChanged(nameof(Cod));
            }
        }

        public string CodOnline
        {
            get
            {
                return _importedMed.CodOnline;
            }
            set
            {
                _importedMed.CodOnline = value;
                OnPropertyChanged(nameof(CodOnline));
            }
        }

        public string CodIntern
        {
            get
            {
                return _importedMed.CodIntern;
            }
            set
            {
                _importedMed.CodIntern = value;
                OnPropertyChanged(nameof(CodIntern));
            }
        }

        public string Denumire
        {
            get
            {
                return _importedMed.Denumire;
            }
            set
            {
                _importedMed.Denumire = value;
                OnPropertyChanged(nameof(Denumire));
            }
        }

        public string DenumireScurta
        {
            get
            {
                return _importedMed.DenumireScurta;
            }
            set
            {
                _importedMed.DenumireScurta = value;
                OnPropertyChanged(nameof(DenumireScurta));
            }
        }

        public string Producator
        {
            get
            {
                return _importedMed.Producator;
            }
            set
            {
                _importedMed.Producator = value;
                OnPropertyChanged(nameof(Producator));
            }
        }

        public decimal? Tva
        {
            get
            {
                return _importedMed.tva;
            }
            set
            {
                _importedMed.tva = value;
                OnPropertyChanged(nameof(Tva));
            }
        }

        public string Tip
        {
            get
            {
                return _importedMed.tip;
            }
            set
            {
                _importedMed.tip = value;
                OnPropertyChanged(nameof(Tip));
            }
        }

        public DateTime? DataInceput
        {
            get
            {
                return _importedMed.DataInceput;
            }
            set
            {
                _importedMed.DataInceput = value;
                OnPropertyChanged(nameof(DataInceput));
            }
        }

        public DateTime? DataSfarsit
        {
            get
            {
                return _importedMed.DataSfarsit;
            }
            set
            {
                _importedMed.DataSfarsit = value;
                OnPropertyChanged(nameof(DataSfarsit));
            }
        }

        public string CodBareProducator
        {
            get
            {
                return _importedMed.codBareProducator;
            }
            set
            {
                _importedMed.codBareProducator = value;
                OnPropertyChanged(nameof(CodBareProducator));
            }
        }

        public string Um
        {
            get
            {
                return _importedMed.um;
            }
            set
            {
                _importedMed.um = value;
                OnPropertyChanged(nameof(Um));
            }
        }

        public decimal? CantMinima
        {
            get
            {
                return _importedMed.cantMinima;
            }
            set
            {
                _importedMed.cantMinima = value;
                OnPropertyChanged(nameof(CantMinima));
            }
        }

        public decimal? PretAmanunt
        {
            get
            {
                return _importedMed.PretAmanunt;
            }
            set
            {
                _importedMed.PretAmanunt = value;
                OnPropertyChanged(nameof(PretAmanunt));
            }
        }

        public string Categorie
        {
            get
            {
                return _importedMed.Categorie;
            }
            set
            {
                _importedMed.Categorie = value;
                OnPropertyChanged(nameof(Categorie));
            }
        }

        public string Link
        {
            get
            {
                return _importedMed.Link;
            }
            set
            {
                _importedMed.Link = value;
                OnPropertyChanged(nameof(Link));
            }
        }

        public string Sursa
        {
            get
            {
                return _importedMed.Sursa;
            }
            set
            {
                _importedMed.Sursa = value;
                OnPropertyChanged(nameof(Sursa));
            }
        }

        public bool? InStocFurnizor
        {
            get
            {
                return _importedMed.InStocFurnizor;
            }
            set
            {
                _importedMed.InStocFurnizor = value;
                OnPropertyChanged(nameof(InStocFurnizor));
            }
        }

        public bool? EsteElaborabil
        {
            get
            {
                return _importedMed.EsteElaborabil;
            }
            set
            {
                _importedMed.EsteElaborabil = value;
                OnPropertyChanged(nameof(EsteElaborabil));
            }
        }

        public decimal? FractieIntreg
        {
            get
            {
                return _importedMed.fractieIntreg;
            }
            set
            {
                _importedMed.fractieIntreg = value;
                OnPropertyChanged(nameof(FractieIntreg));
            }
        }

        public string TipImpachetare
        {
            get
            {
                return _importedMed.TipImpachetare;
            }
            set
            {
                _importedMed.TipImpachetare = value;
                OnPropertyChanged(nameof(TipImpachetare));
            }
        }

        public decimal? UltimulPretDeIntrare
        {
            get
            {
                return _importedMed.UltimulPretDeIntrare;
            }
            set
            {
                _importedMed.UltimulPretDeIntrare = value;
                OnPropertyChanged(nameof(UltimulPretDeIntrare));
            }
        }

        public string UltimulFurnizor
        {
            get
            {
                return _importedMed.UltimulFurnizor;
            }
            set
            {
                _importedMed.UltimulFurnizor = value;
                OnPropertyChanged(nameof(UltimulFurnizor));
            }
        }

        public decimal? PenUltimulPretDeIntrare
        {
            get
            {
                return _importedMed.PenUltimulPretDeIntrare;
            }
            set
            {
                _importedMed.PenUltimulPretDeIntrare = value;
                OnPropertyChanged(nameof(PenUltimulPretDeIntrare));
            }
        }

        public string PenUltimulFurnizor
        {
            get
            {
                return _importedMed.PenUltimulFurnizor;
            }
            set
            {
                _importedMed.PenUltimulFurnizor = value;
                OnPropertyChanged(nameof(PenUltimulFurnizor));
            }
        }

        public bool? Fractionabil
        {
            get
            {
                return _importedMed.Fractionabil;
            }
            set
            {
                _importedMed.Fractionabil = value;
                OnPropertyChanged(nameof(Fractionabil));
            }
        }

        public bool? EsteVizibilLaVanzare
        {
            get
            {
                return _importedMed.EsteVizibilLaVanzare;
            }
            set
            {
                _importedMed.EsteVizibilLaVanzare = value;
                OnPropertyChanged(nameof(EsteVizibilLaVanzare));
            }
        }

        public bool? VizibilOnline
        {
            get
            {
                return _importedMed.VizibilOnline;
            }
            set
            {
                _importedMed.VizibilOnline = value;
                OnPropertyChanged(nameof(VizibilOnline));
            }
        }

        public bool? VizibilComandaAndroid
        {
            get
            {
                return _importedMed.VizibilComandaAndroid;
            }
            set
            {
                _importedMed.VizibilComandaAndroid = value;
                OnPropertyChanged(nameof(VizibilComandaAndroid));
            }
        }

        public decimal? PretEuro
        {
            get
            {
                return _importedMed.PretEuro;
            }
            set
            {
                _importedMed.PretEuro = value;
                OnPropertyChanged(nameof(PretEuro));
            }
        }


        private string _treatmentQuantity;
        public string TreatmentQuantity
        {
            get => _treatmentQuantity;
            set
            {
                _treatmentQuantity = value;
                OnPropertyChanged(nameof(TreatmentQuantity));
            }
        }

        //private string _quantityString;
        //public string QuantityString
        //{
        //    get => _quantityString;
        //    set
        //    {
        //        decimal parsed;
        //        if (decimal.TryParse(value, out parsed))
        //        {
        //            _quantityString = value;
        //            TreatmentQuantity = parsed;
        //            OnPropertyChanged(nameof(QuantityString));
        //        }
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
