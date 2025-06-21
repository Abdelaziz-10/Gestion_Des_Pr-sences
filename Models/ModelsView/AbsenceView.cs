using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Des_prèneces.Models.ModelsView
{
    public class AbsenceView
    {
        public int IdAbsence { get; set; }
        public DateTime? DateDebutAb { get; set; }
        public DateTime? DateFinAb { get; set; }
        public int? NbHAb { get; set; }
        public string NomCl { get; set; }
        public string Prenom { get; set; }
        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
