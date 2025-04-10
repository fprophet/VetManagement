﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class TreatmentMed 
    {
        public int MedId { get; set; }
        
        public int TreatmentId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        //for historically purposes
        public DateTime Valability { get; set; }

        public Treatment Treatment { get; set; }

        public Med Med { get; set; }
    }
}
