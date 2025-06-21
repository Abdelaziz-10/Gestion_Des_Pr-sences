using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Disponibilité
    {
        public int IdDisponibilité { get; set; }
        public DateTime? DateMiseEnDisponibilité { get; set; }
        public DateTime? DateHDebutDisponibilité { get; set; }
        public DateTime? DateHFinDisponibilité { get; set; }
        public int? NumCl { get; set; }

        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
