using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class ImportedMed
    {
        // Properties are nullable because the data from the Excel file might be missing
        //id is string because it won't be incremented in the database and some numbers are listed with commas
        public string? Id { get; set; }

        public string? Cod { get; set; }

        public string? CodOnline { get; set; }

        public string? CodIntern { get; set; }

        public string? Denumire { get; set; }

        public string? DenumireScurta { get; set; }

        public string? Producator { get; set; }

        public decimal? tva { get; set; }

        public string? tip { get; set; }

        public DateTime? DataInceput { get; set; }

        public DateTime? DataSfarsit { get; set; }

        public string? codBareProducator { get; set; }

        public string? um { get; set; }

        public decimal? cantMinima { get; set; }

        public decimal? PretAmanunt { get; set; }

        public string? Categorie { get; set; }

        public string? Link { get; set; }

        public string? Sursa { get; set; }

        public bool? InStocFurnizor { get; set; }

        public bool? EsteElaborabil { get; set; }

        public decimal? fractieIntreg { get; set; }

        public string? TipImpachetare { get; set; }

        public decimal? UltimulPretDeIntrare { get; set; }

        public string? UltimulFurnizor { get; set; }

        public decimal? PenUltimulPretDeIntrare { get; set; }

        public string? PenUltimulFurnizor { get; set; }

        public bool? Fractionabil { get; set; }

        public bool? EsteVizibilLaVanzare { get; set; }

        public bool? VizibilOnline { get; set; }

        public bool? VizibilComandaAndroid { get; set; }

        public decimal? PretEuro { get; set; }

        public List<TreatmentImportedMed> TreatmentImportedMeds { get; set; } = new List<TreatmentImportedMed>();

    }
}
