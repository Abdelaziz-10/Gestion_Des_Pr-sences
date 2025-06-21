using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models.ModelsView
{
    public partial class CongéView
    {
        public int IdCongé { get; set; }
        public DateTime? DateDebutCongé { get; set; }
        public DateTime? DateFinCongé { get; set; }
        public string Types { get; set; }
        public string Accord { get; set; }
        public string NomCl { get; set; }
        public string Prenom { get; set; }
        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
