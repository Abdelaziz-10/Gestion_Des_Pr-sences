using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Congé
    {
        public int IdCongé { get; set; }
        public DateTime? DateDebutCongé { get; set; }
        public DateTime? DateFinCongé { get; set; }
        public string Types { get; set; }
        public int? NumCl { get; set; }
        public string Accord { get; set; }

        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
