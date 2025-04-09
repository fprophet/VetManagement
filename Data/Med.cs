using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace VetManagement.Data
{
    public class Med : BaseEntity
    {

        [Required(ErrorMessage = "Denumirea produsului este obligatorie!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tipul produsului este obligatoriu!"),AllowedValues("medicament", "vaccin")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Tipul gramajului este obligatoriu!")]
        public string MeasurmentUnit { get; set; }

        [Required(ErrorMessage = "Tipul stocului este obligatoriu!"),Column("PieceType"),AllowedValues("comprimate", "flacoane"),]
        public string PieceType { get; set; }

        [Required(ErrorMessage = "Cantitatea în bucăți trebuie să fie mai mare ca 0!")]
        public int Pieces { get; set; }

        [Required(ErrorMessage = "Cantitatea per bucată trebuie să fie mai mare ca 0!")]
        public decimal PerPiece { get; set; }

        [Required(ErrorMessage = "Numele furnizorului este obligatoriu!")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "Numărul facturii este obligatoriu!")]
        public int InvoiceNumber { get; set; }

        public string WaitingTime { get; set; }

        public string Administration { get; set; }

        public decimal TotalAmount { get; set; }

        public string? LotID { get; set; }

        [Required(ErrorMessage = "Valabilitatea trebuie să fie o dată în viitor!")]
        public DateTime Valability { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? LastUsed { get; set; }

        public string? Description { get; set; }

        public List<TreatmentMed> TreatmentMeds { get; set; } = new List<TreatmentMed>();

        public Invoice Invoice { get; set; }

        public string Unit => PieceType == "comprimate" ? "pastilă" : "ml";

        //public string UnitPerPiece => PieceType == "comprimate" ? "-" : PerPiece + "ml/flacon";

        public string UnitPerPiece => MeasurmentUnit + "/" + SingularPieceType;

        public string PerPieceAndUnit => PerPiece + " " + UnitPerPiece;

        public string SingularPieceType => PieceType == "comprimate" ? "comprimat" : "flacon";

    }
}
