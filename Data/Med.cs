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
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string Name { get; set; }

        [AllowedValues("medicament", "vaccin")]
        public string Type { get; set; }

        [Column("PieceType"),AllowedValues("comprimate", "flacoane")]
        public string PieceType { get; set; }

        [Required(ErrorMessage = "Cantitatea în bucăți trebuie să fie mai mare ca 0!")]
        public float Pieces { get; set; }

        [Required(ErrorMessage = "Cantitatea per bucată trebuie să fie mai mare ca 0!")]
        public float PerPiece { get; set; }

        private float _totalAmount;
        public float TotalAmount 
        { 
            get => _totalAmount; 
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public string? LotID { get; set; }

        [Required(ErrorMessage = "Valabilitatea trebuie să fie o data în viitor!")]
        public long Valability { get; set; }

        public string? Description { get; set; }

        public long DateAdded { get; set; }

        public long DateUpdated { get; set; }

        public List<TreatmentMed> TreatmentMeds { get; set; } = new List<TreatmentMed>();

        public DateTime DateAddedFormated => DateTimeOffset.FromUnixTimeSeconds(DateAdded).UtcDateTime;

        public string ValabilityFormated => 
            TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(Valability).UtcDateTime, TimeZoneInfo.Local).Date.ToString("yyyy-MM-dd");

        public string Unit => PieceType == "comprimate" ? "comprimate" : "ml";
        public string UnitPerPiece => PieceType == "comprimate" ? "-" : PerPiece + "ml/flacon";



        //public IEnumerable<ValidationResult> Valdiate(ValidationContext validationContext)
        //{
        //    var errors =  new List<ValidationResult>();

        //    if()
        //}
    }
}
