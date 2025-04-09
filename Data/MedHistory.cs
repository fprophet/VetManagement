using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class MedHistory : BaseEntity
    {

        public int OriginalId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string MeasurmentUnit { get; set; }

        [Required]
        public string PieceType { get; set; }

        [Required]
        public int Pieces { get; set; }

        [Required]
        public decimal PerPiece { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public int InvoiceNumber { get; set; }

        public string WaitingTime { get; set; }

        public string Administration { get; set; }

        public string? LotID { get; set; }

        [Required]
        public DateTime Valability { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateDeleted { get; set; }

        public DateTime? LastUsed { get; set; }

        public string? Description { get; set; }

        public string Unit => PieceType == "comprimate" ? "pastilă" : "ml";

        public string UnitPerPiece => MeasurmentUnit + "/" + SingularPieceType;

        public string PerPieceAndUnit => PerPiece + " " + UnitPerPiece;

        public string SingularPieceType => PieceType == "comprimate" ? "comprimat" : "flacon";

    }
}
