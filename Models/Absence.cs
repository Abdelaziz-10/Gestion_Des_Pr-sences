using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Absence
    {
        public int IdAbsence { get; set; }
        public DateTime? DateDebutAb { get; set; }
        public DateTime? DateFinAb { get; set; }
        public int? NbHAb { get; set; }
        public int? NumCl { get; set; }

        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
