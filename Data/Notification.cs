using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Notification : BaseEntity
    {
        [StringLength(20)]
        public string Type { get; set; }

        // user, manager, all
        [StringLength(20)]
        public string UserType { get; set; }

        [StringLength(300)]
        public string Message { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime ReadAt { get; set; }

        public bool Read { get; set; }

    }
}
